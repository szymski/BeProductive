﻿using BeProductive.Modules.Common.Helpers;
using BeProductive.Modules.Goals.Presentation.Components;
using BeProductive.Modules.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Infrastructure;

public class GoalService {
    private readonly IDbContextFactory<AppContext> _contextFactory;
    private readonly ILogger<GoalService> _logger;
    private readonly AuthService _authService;

    protected int UserId => _authService.GetAuthData()!.UserId;

    public GoalService(
        ILogger<GoalService> logger,
        IDbContextFactory<AppContext> contextFactory,
        AuthService authService)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _authService = authService;
    }
    
    public event Action GoalsUpdated;

    public async Task<List<Goal>> GetGoals()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("User id {Id}", UserId);

        return await context.Goals
            .Where(goal => goal.UserId == UserId)
            .OrderBy(goal => goal.Order)
            .ToListAsync();
    }

    public async Task<IDictionary<string, Goal>> GetSystemGoalsByTypes(IEnumerable<string> types)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("User id {Id}", UserId);

        return await context.Goals
            .Where(goal => goal.UserId == UserId)
            .Where(goal => goal.IsSystem && goal.SystemType != null && types.Contains(goal.SystemType))
            .OrderBy(goal => goal.Order)
            .ToDictionaryAsync(goal => goal.SystemType!);
    }

    public async Task<Goal?> GetGoal(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Goals
            .Where(x => x.UserId == UserId)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Goal> UpdateGoal(Goal goal)
    {
        goal.Validate();

        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("Updating goal {@Goal}", goal);
        context.Goals.Update(goal);
        await context.SaveChangesAsync();
        return goal;
    }

    public async Task<Goal> AddGoal(Goal goal)
    {
        goal.Validate();

        goal.UserId = UserId;

        await using var context = await _contextFactory.CreateDbContextAsync();

        var count = await context.Goals
            .Where(x => x.UserId == UserId)
            .CountAsync();
        goal.Order = count;
        await context.Goals.AddAsync(goal);
        await context.SaveChangesAsync();
        _logger.LogInformation("Added new goal {@Goal}", goal);
        
        GoalsUpdated?.Invoke();

        return goal;
    }

    public async Task<Goal> AddSystemGoal(Goal goal, string type)
    {
        // TODO

        goal.UserId = UserId;
        goal.IsSystem = true;
        goal.SystemType = type;

        await using var context = await _contextFactory.CreateDbContextAsync();

        var count = await context.Goals
            .Where(x => x.UserId == UserId)
            .CountAsync();
        goal.Order = count;
        await context.Goals.AddAsync(goal);
        await context.SaveChangesAsync();
        _logger.LogInformation("Added new goal {@Goal}", goal);

        return goal;
    }

    public async Task DeleteGoal(Goal goal)
    {
        if (goal.IsSystem)
        {
            _logger.LogWarning("Attempted to delete system goal {@Goal}", goal);
            throw new InvalidOperationException("Cannot delete system goal");
        }

        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("Deleting goal {@Goal}", goal);
        context.Goals.Remove(goal);
        await context.SaveChangesAsync();
        
        GoalsUpdated?.Invoke();
    }

    public async Task<GoalDayState?> GetStateForDay(Goal goal, DateOnly date)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.GoalDayStates
            .Where(x => x.GoalId == goal.Id)
            .Where(x => x.Day == date)
            .SingleOrDefaultAsync();
    }

    public async Task<IDictionary<Goal, GoalDayState?>> GetManyGoalsStatesForDay(IEnumerable<Goal> goals, DateOnly date)
    {
        var goalIdMap = goals.ToDictionary(goal => goal.Id);
        var goalIds = goalIdMap.Keys;

        await using var context = await _contextFactory.CreateDbContextAsync();

        var states = await context.GoalDayStates
            .Where(x => goalIds.Contains(x.GoalId))
            .Where(x => x.Day == date)
            .ToListAsync();

        return goalIdMap.Values
            .ToDictionary(goal => goal,
                          goal => states.SingleOrDefault(state => state.GoalId == goal.Id));
    }

    public async Task<IReadOnlyList<GoalDayState>> GetStatesForMonth(Goal goal, DateTime date)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entry = await context.Goals
            .Where(x => x.UserId == UserId)
            .SingleAsync(x => x.Id == goal.Id);

        var (firstDay, lastDate) = DateHelper.FirstAndLastDayOfMonth(DateOnly.FromDateTime(date));

        return await context.Entry(entry)
            .Collection(goal => goal.GoalDayStates)
            .Query()
            .Where(state => state.Day >= firstDay && state.Day <= lastDate)
            .ToListAsync();
    }

    public async Task<GoalDayState?> GetLastDayState(Goal goal)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entry = await context.Goals
            .Where(x => x.UserId == UserId)
            .SingleAsync(x => x.Id == goal.Id);

        return await context.Entry(entry)
            .Collection(goal => goal.GoalDayStates)
            .Query()
            .OrderByDescending(goal => goal.Day)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateOrders(IEnumerable<KeyValuePair<Goal, int>> goalOrders)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var goalIds = goalOrders.Select(x => x.Key);

        var goals = await context.Goals
            .Where(x => x.UserId == UserId)
            .Where(x => goalIds.Contains(x))
            .ToArrayAsync();

        foreach (var (goal, order) in goalOrders)
        {
            var entity = goals.Single(r => r.Id == goal.Id);
            entity.Order = order;
        }

        await context.SaveChangesAsync();
    }
}