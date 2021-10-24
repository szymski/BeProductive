namespace BeProductive.Modules.Goals.Infrastructure;

public class GoalService
{
    private readonly List<Goal> _goals = new()
    {
        new Goal() { Name = "Test goal" },
        new Goal() { Name = "Zwalić gruchę raz dziennie" },
        new Goal() { Name = "Wykonać poranny rytuał" },
        new Goal() { Name = "Obrazić kogoś" },
        new Goal() { Name = "Dupa" },
    };
    
    public List<Goal> GetGoals()
    {
        return _goals;
    }

    public Goal AddGoal(Goal goal)
    {
        _goals.Add(goal);
        return goal;
    }
}