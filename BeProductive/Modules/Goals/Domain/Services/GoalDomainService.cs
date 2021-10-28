using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Domain.Services;

public class GoalDomainService
{
    private readonly AppContext _context;

    public GoalDomainService(AppContext context)
    {
        _context = context;
    }

    public async Task<bool> SetStateForDay(Goal goal, DateOnly day, GoalState state)
    {
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