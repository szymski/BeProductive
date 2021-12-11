using System.ComponentModel.DataAnnotations;
using BeProductive.Modules.Common.Domain;
using BeProductive.Modules.GoalExtensions.Description.Domain;
using BeProductive.Modules.GoalExtensions.EmergencyGoal.Domain;
using BeProductive.Modules.Goals.Domain;

public class Goal : BaseOwnedEntity
{
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsSystem { get; set; }
    public string? SystemType { get; set; }
    public AllowedDaysOfWeek AllowedDaysOfWeek { get; set; } = AllowedDaysOfWeek.All;

    public List<GoalDayState> GoalDayStates { get; set; }

    public EmergencyGoal? EmergencyGoal { get; set; }
    public GoalDescription? GoalDescription { get; set; }

    public void Validate()
    {
        if (AllowedDaysOfWeek == AllowedDaysOfWeek.None)
            throw new ValidationException("At least one day of week must be allowed");
    }

    public override string ToString()
    {
        return $"Goal {{ {nameof(Id)} = {Id} }}";
    }
}