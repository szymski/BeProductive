using BeProductive.Modules.Goals.Domain.Entities;

namespace BeProductive.Modules.GoalExtensions.EmergencyGoal.Domain;

public class EmergencyGoal : GoalExtensionBaseEntity
{
    public string Title { get; set; }
}