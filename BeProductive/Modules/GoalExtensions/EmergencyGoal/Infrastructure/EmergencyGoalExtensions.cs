namespace BeProductive.Modules.GoalExtensions.EmergencyGoal.Infrastructure;

public static class EmergencyGoalExtensions
{
    public static bool HasEmergencyGoal(this Goal goal)
        => goal.EmergencyGoal is not null;
}