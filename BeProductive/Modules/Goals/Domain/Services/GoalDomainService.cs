using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Domain.Services;

public class GoalDomainService
{
    private readonly AppContext _context;
    private readonly ILogger<GoalDomainService> _logger;

    public GoalDomainService(AppContext context, ILogger<GoalDomainService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> SetStateForDay(Goal goal, DateOnly day, GoalState state)
    {
        _logger.LogInformation("Setting goal {@Goal} state for day {Day} to {State}", goal, day, state);
        
        var entry = await _context.Entry(goal)
            .Collection(goal => goal.GoalDayStates)
            .Query()
            .SingleOrDefaultAsync(goal => goal.Day == day);

        if (state == GoalState.Unknown)
        {
            if (entry is null) return false;
            
            _context.GoalDayStates.Remove(entry);
        }
        else
        {
            if (entry?.State == state)
            {
                return false;
            }
            
            entry ??= new GoalDayState()
            {
                Goal = goal,
                Day = day,
                State = state,
            };

            entry.State = state;

            _context.GoalDayStates.Update(entry);
        }
        
        await _context.SaveChangesAsync();

        return true;
    }
}