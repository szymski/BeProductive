using BeProductive.Modules.Goals.Domain;

namespace BeProductive.Modules.Goals.Infrastructure;

public class GoalService
{
    private readonly List<Goal> _goals = new()
    {
        new() { Name = "Test goal", Color = GoalColors.Colors[1], Icon = GoalIcons.Icons[1] },
        new() { Name = "Zwalić gruchę raz dziennie", Color = GoalColors.Colors[2], Icon = GoalIcons.Icons[2] },
        new() { Name = "Wykonać poranny rytuał", Color = GoalColors.Colors[3] },
        new() { Name = "Obrazić kogoś", Color = GoalColors.Colors[4], Icon = GoalIcons.Icons[8] },
        new() { Name = "Dupa", Color = GoalColors.Colors[5] },
    };

    public List<Goal> GetGoals()
    {
        return _goals;
    }

    public Goal? GetGoal(int id)
    {
        return _goals.SingleOrDefault(goal => goal.Id == id);
    }

    public Goal SaveGoal(Goal goal)
    {
        return goal;
    }

    public Goal AddGoal(Goal goal)
    {
        _goals.Add(goal);
        return goal;
    }
}