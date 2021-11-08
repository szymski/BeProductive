using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Domain.Services;

public class GoalDomainService
{
    private readonly IDbContextFactory<AppContext> _contextFactory;
    private readonly ILogger<GoalDomainService> _logger;

    public GoalDomainService(ILogger<GoalDomainService> logger, IDbContextFactory<AppContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> SetStateForDay(Goal goal, DateOnly day, GoalState state)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var goalEntry = await context.Goals.FindAsync(goal.Id);
        
        _logger.LogInformation("Setting goal {@Goal} state for day {Day} to {State}", goal, day, state);
        
        var entry = await context.Entry(goalEntry)
            .Collection(goal => goal.GoalDayStates)
            .Query()
            .SingleOrDefaultAsync(goal => goal.Day == day);

        if (state == GoalState.Unknown)
        {
            if (entry is null) return false;
            
            context.GoalDayStates.Remove(entry);
        }
        else
        {
            if (entry?.State == state)
            {
                return false;
            }
            
            entry ??= new()
            {
                Goal = goalEntry,
                Day = day,
                State = state, 
            };

            entry.State = state;

            context.GoalDayStates.Update(entry);
        }
        
        await context.SaveChangesAsync();

        return true;
    }
}