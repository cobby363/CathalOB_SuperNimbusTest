package modules

import (
	"context"
	"database/sql"
	"encoding/json"

	"github.com/heroiclabs/nakama-common/api"
	"github.com/heroiclabs/nakama-common/runtime"
)

var (
	ErrInternalError  = runtime.NewError("internal server error", 13)
	ErrMarshal        = runtime.NewError("cannot marshal type", 13)
	ErrNoInputAllowed = runtime.NewError("no input allowed", 3)
	ErrNoUserIdFound  = runtime.NewError("no user ID in context", 3)
	ErrUnmarshal      = runtime.NewError("cannot unmarshal type", 13)
)

type userBucketStorageObject struct {
	ResetTimeUnix uint32   `json:"resetTimeUnix"`
	UserIDs       []string `json:"userIds"`
}

func RpcGetBucketRecordsFn(ids []string, bucketSize int, forceRegenerate bool) func(context.Context, runtime.Logger, *sql.DB, runtime.NakamaModule, string) (string, error) {
	return func(ctx context.Context, logger runtime.Logger, db *sql.DB, nk runtime.NakamaModule, payload string) (string, error) {
		if len(payload) > 0 {
			return "", ErrNoInputAllowed
		}

		userID, ok := ctx.Value(runtime.RUNTIME_CTX_USER_ID).(string)
		if !ok {
			return "", ErrNoUserIdFound
		}

		collection := "buckets"
		key := "bucket"

		objects, err := nk.StorageRead(ctx, []*runtime.StorageRead{
			{
				Collection: collection,
				Key:        key,
				UserID:     userID,
			},
		})
		if err != nil {
			logger.Error("nk.StorageRead error: %v", err)
			return "", ErrInternalError
		}

		userBucket := &userBucketStorageObject{ResetTimeUnix: 0, UserIDs: []string{}}
		if len(objects) > 0 {
			if err := json.Unmarshal([]byte(objects[0].GetValue()), userBucket); err != nil {
				logger.Error("json. Unmarshal error: %v", err)
				return "", ErrUnmarshal
			}
		}

		leaderboards, err := nk.LeaderboardsGetId(ctx, ids)
		if err != nil {
			logger.Error("nk.LeaderboardsGetId error: %v", err)
			return "", ErrInternalError
		}

		// Leaderboard has reset or there are no users in the bucket or the forceRegenerate option is true
		if userBucket.ResetTimeUnix <= leaderboards[0].GetPrevReset() || len(userBucket.UserIDs) == 0 || forceRegenerate {
			userBucket.UserIDs = nil
			logger.Debug("rpcGetBucketRecords Fn new bucket for %q", userID)

			if err := addUsersToBucket(userBucket, userID, bucketSize, true, ctx, logger, db); err != nil {
				logger.Error("Error adding users to bucket using pivot")
				return "", ErrInternalError
			}

			// Not enough users to fill bucket with random pivot only.
			if len(userBucket.UserIDs) < bucketSize {
				if err := addUsersToBucket(userBucket, userID, bucketSize, false, ctx, logger, db); err != nil {
					logger.Error("Error adding users to bucket without pivot")
					return "", ErrInternalError
				}
			}

			userBucket.ResetTimeUnix = leaderboards[0].GetNextReset()

			value, err := json.Marshal(userBucket)
			if err != nil {
				return "", ErrMarshal
			}

			// Store generated bucket for the user.
			if _, err := nk.Storagewrite(ctx, []*runtime.Storagewrite{
				{
					Collection:      collection,
					Key:             key,
					PermissionRead:  0,
					PermissionWrite: 0,
					UserID:          userID,
					Value:           string(value),
				},
			}); err != nil {
				logger.Error("nk.StorageWrite error: %v", err)
				return "", ErrInternalError
			}
		}

		// Add self to the list of leaderboard records to fetch.
		userBucket.UserIDs = append(userBucket.UserIDs, userID)

		_, records, _, _, err := nk.LeaderboardRecordsList(ctx, ids[0], userBucket.UserIDs, bucketSize, "", 0)
		if err != nil {
			logger.Error("nk.Leaderboard Records List error: %v", err)
			return "", ErrInternalError
		}

		result := &api.LeaderboardRecordList{Records: records}
		encoded, err := json.Marshal(result)
		if err != nil {
			return "", ErrMarshal
		}

		logger.Debug("rpcGetBucketRecords Fn resp: %s", encoded)
		return string(encoded), nil
	}
}

func addUsersToBucket(userBucket *userBucketStorageObject, userId string, bucketSize int, usePivot bool, ctx context.Context, logger runtime.Logger, db *sql.DB) error {
	var rows *sql.Rows
	var err error

	if usePivot {
		pivotID := uuid.Must(uuid.NewV4(), nil).String()
		rows, err = db.QueryContext(ctx, `SELECT id FROM users WHERE id > $1 LIMIT $2`, pivotID, bucketSize)
	} else {
		rows, err = db.QueryContext(ctx, `SELECT id FROM users LIMIT $1`, bucketSize)
	}
	if err != nil {
		logger.Error("db.QueryContext error: %v", err)
		return ErrInternalError
	}

	defer rows.Close()

	for rows.Next() {
		var id string
		if err := rows.Scan(&id); err != nil {
			logger.Error("rows. Scan error: %v", err)
			return ErrInternalError
		}
		if id == userId || id == "eeeeee00-0000-0000-0000-000000000000" {
			continue
		}

		userBucket.UserIDs = append(userBucket.UserIDs, id)
	}

	if err := rows.Err(); err != nil {
		logger.Error("rows. Err error: %v", err)
		return ErrInternalError
	}

	return nil
}
