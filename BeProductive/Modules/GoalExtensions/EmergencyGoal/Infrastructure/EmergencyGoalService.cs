using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.GoalExtensions.EmergencyGoal.Infrastructure;

public class EmergencyGoalService
{
    private ILogger<EmergencyGoalService> _logger;
    private IDbContextFactory<AppContext> _contextFactory;

    public EmergencyGoalService(
        ILogger<EmergencyGoalService> logger,
        IDbContextFactory<AppContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<Domain.EmergencyGoal> UpdateEmergencyGoal(Goal goal, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required", nameof(title));
        }
        
        _logger.LogInformation("Updating emergency goal for goal {@Goal} to {Title}", goal, title);
        
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entry = await context.EmergencyGoals
            .SingleOrDefaultAsync(eg => eg.GoalId == goal.Id);

        entry ??= new()
        {
            GoalId = goal.Id,
            Title = title,
        };

        entry.Title = title;

        context.Update(entry);

        await context.SaveChangesAsync();

        return entry;
    }

    public async Task DeleteEmergencyGoal(Goal goal)
    {
        _logger.LogInformation("Deleting emergency goal for goal {@Goal}", goal);

        await using var context = await _contextFactory.CreateDbContextAsync();

        var entry = await context.EmergencyGoals
            .SingleAsync(eg => eg.GoalId == goal.Id);

        context.Remove(entry);

        await context.SaveChangesAsync();
    }
}