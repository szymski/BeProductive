﻿using BeProductive.Modules.Goals.Domain;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Infrastructure;

public class GoalStatsService
{
    private AppContextFactory _contextFactory;
    private ILogger<GoalStatsService> _logger;

    public GoalStatsService(
        AppContextFactory contextFactory,
        ILogger<GoalStatsService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    private const string GetCurrentStreakSql = @"WITH last_failed AS (
    SELECT ""Day""
    FROM ""GoalDayStates"" ls
    WHERE ls.""GoalId"" = @GoalId
        AND ls.""State"" <> 'Success'
        AND ls.""State"" <> 'NotApplicable'
    ORDER BY ls.""Day"" DESC
    LIMIT 1
),
states AS (
    SELECT *
    FROM ""GoalDayStates"" s
    WHERE s.""GoalId"" = @GoalId
        AND ((SELECT COUNT(""Day"") FROM last_failed) = 0 OR S.""Day"" > (SELECT ""Day"" FROM last_failed))
        AND s.""State"" <> 'NotApplicable'
    ORDER BY s.""Day"" DESC
)
SELECT count(*)
FROM states
";

    // TODO: When selecting goal, this is called twice
    public async Task<int> GetCurrentStreak(Goal goal)
    {
        _logger.LogDebug("Getting current streak for goal {@Goal}", goal);

        await using var context = await _contextFactory.CreateDbContextAsync();
        var connection = context.Database.GetDbConnection();

        var streak = await connection.QuerySingleAsync<int>(GetCurrentStreakSql, new
        {
            GoalId = goal.Id,
        });

        return streak;
    }
}