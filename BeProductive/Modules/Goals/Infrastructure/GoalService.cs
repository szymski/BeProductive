using BeProductive.Modules.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Infrastructure;

public class GoalService
{
    private readonly IDbContextFactory<AppContext> _contextFactory;
    private readonly ILogger<GoalService> _logger;

    public GoalService(ILogger<GoalService> logger, IDbContextFactory<AppContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<List<Goal>> GetGoals()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Goals
            .OrderBy(goal => goal.Order)
            .ToListAsync();
    }

    public async Task<Goal?> GetGoal(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Goals.FindAsync(id);
    }

    public async Task<Goal> UpdateGoal(Goal goal)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("Updating goal {@Goal}", goal);
        context.Goals.Update(goal);
        await context.SaveChangesAsync();
        return goal;
    }

    public async Task<Goal> AddGoal(Goal goal)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var count = await context.Goals.CountAsync();
        goal.Order = count;
        await context.Goals.AddAsync(goal);
        await context.SaveChangesAsync();
        _logger.LogInformation("Added new goal {@Goal}", goal);
        
        return goal;
    }

    public async Task DeleteGoal(Goal goal)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("Deleting goal {@Goal}", goal);
        context.Goals.Remove(goal);
        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<GoalDayState>> GetStatesForMonth(Goal goal, DateTime date)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entry = await context.Goals.FindAsync(goal.Id);
        
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

        var entry = await context.Goals.FindAsync(goal.Id);
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
            .Where(goal => goalIds.Contains(goal))
            .ToArrayAsync();

        foreach (var (goal, order) in goalOrders)
        {
            var entity = goals.Single(r => r.Id == goal.Id);
            entity.Order = order;
        }

        await context.SaveChangesAsync();
    }
}