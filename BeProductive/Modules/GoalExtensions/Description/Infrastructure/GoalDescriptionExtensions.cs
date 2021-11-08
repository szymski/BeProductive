namespace BeProductive.Modules.GoalExtensions.EmergencyGoal.Infrastructure;

public static class GoalDescriptionExtensions
{
    public static bool HasDescription(this Goal goal)
        => goal.GoalDescription is not null;
}