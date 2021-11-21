using BeProductive.Modules.Common.Helpers;
using BeProductive.Modules.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Infrastructure;

public class GoalService
{
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

    public async Task<List<Goal>> GetGoals()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("User id {Id}", UserId);
        
        return await context.Goals
            .Where(goal => goal.UserId == UserId)
            .OrderBy(goal => goal.Order)
            .ToListAsync();
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
        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("Updating goal {@Goal}", goal);
        context.Goals.Update(goal);
        await context.SaveChangesAsync();
        return goal;
    }

    public async Task<Goal> AddGoal(Goal goal)
    {
        goal.UserId = UserId;

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
    
    public async Task<Goal> AddSystemGoal(Goal goal, string type)
    {
        // TODO
        
        goal.UserId = UserId;

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
        await using var context = await _contextFactory.CreateDbContextAsync();

        _logger.LogInformation("Deleting goal {@Goal}", goal);
        context.Goals.Remove(goal);
        await context.SaveChangesAsync();
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