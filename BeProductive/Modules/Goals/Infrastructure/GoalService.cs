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

    public async Task<List<Goal>> GetGoals()
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
}