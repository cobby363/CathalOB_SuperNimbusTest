package main

import (
	"context"
	"database/sql"

	"github.com/heroiclabs/bucketed-leaderboards-go/modules"
	"github.com/heroiclabs/nakama-common/runtime"
)

func InitModule(ctx context.Context, logger runtime.Logger, db *sql.DB, nk runtime.NakamaModule, initializer runtime.Initializer) error {
	logger.Info("Go module loaded.")

	leaderboardId := "bucketed_weekly"
	createLeaderboard(leaderboardId, "Bucketed Weekly #1", ctx, logger, nk)

	if err := initializer.RegisterRpc("get_bucket_records", modules.RpcGetBucketRecordsFn([]string{leaderboardId}, 20, false)); err != nil {
		return err
	}

	if err := initializer.RegisterRpc("get_bucket_records_force_regenerate", modules.RpcGetBucketRecordsFn([]string{leaderboardId}, 20, true)); err != nil {
		return err
	}

	return nil
}

func createLeaderboard(id string, title string, ctx context.Context, logger runtime.Logger, nk runtime.NakamaModule) error {
	logger.Info("Creating Leaderboard with ID %q and Title %q", id, title)

	err := nk.LeaderboardCreate(ctx, id, false, "desc", "incr", "0 0 * * 0", nil)
	if err != nil {
		logger.Error(err.Error())
		return err
	}

	logger.Info("Leaderboard %q created successfully", id)
	return nil
}
