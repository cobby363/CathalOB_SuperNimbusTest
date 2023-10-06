const dummyUserDeviceId = 'B1DA5988-FC6F-4B6F-8EA9-217DEEC3CDB6';
const dummyUserDeviceUsername = 'TestUserDevice';

const globalLeaderboard = 'global';
const leaderboardIds = [
    globalLeaderboard,
];

/**
 * Main function.
 */
const InitModule: nkruntime.InitModule =
        function(ctx: nkruntime.Context, logger: nkruntime.Logger, nk: nkruntime.Nakama, initializer: nkruntime.Initializer) {
    // Add at least one user to the system.
    nk.authenticateDevice(dummyUserDeviceId, dummyUserDeviceUsername, true);

    // Set up leaderboards.
    const authoritative = false;
    const metadata = {};
    const scoreOperator = nkruntime.Operator.BEST;
    const sortOrder = nkruntime.SortOrder.ASCENDING;
    const resetSchedule = null;
    leaderboardIds.forEach(id => {
        nk.leaderboardCreate(id, authoritative, sortOrder, scoreOperator, resetSchedule, metadata);
        logger.info('leaderboard %q created', id);
    });
}


