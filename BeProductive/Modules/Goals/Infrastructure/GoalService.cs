using BeProductive.Modules.Common.Helpers;
using BeProductive.Modules.Goals.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Infrastructure;

public class GoalService
{
    private readonly AppContext _context;

    public GoalService(AppContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Goal>> GetGoals()
    {
        return await _context.Goals.ToListAsync();
    }

    public async Task<Goal?> GetGoal(int id)
    {
        return await _context.Goals.FindAsync(id);
    }

    public async Task<Goal> SaveGoal(Goal goal)
    {
        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task<Goal> AddGoal(Goal goal)
    {
        await _context.Goals.AddAsync(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task DeleteGoal(Goal goal)
    {
        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<GoalDayState>> GetStatesForMonth(Goal goal, DateTime date)
    {
        var (firstDay, lastDate) = DateHelper.FirstAndLastDayOfMonth(DateOnly.FromDateTime(date));

        return await _context.Entry(goal)
            .Collection(goal => goal.GoalDayStates)
            .Query()
            .Where(state => state.Day >= firstDay && state.Day <= lastDate)
            .ToListAsync();
    }

    public async Task<GoalDayState?> GetLastDayState(Goal goal)
    {
        return await _context.Entry(goal)
            .Collection(goal => goal.GoalDayStates)
            .Query()
            .OrderByDescending(goal => goal.Day)
            .FirstOrDefaultAsync();
    }
}