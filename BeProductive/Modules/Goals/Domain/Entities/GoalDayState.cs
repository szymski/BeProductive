using BeProductive.Modules.Common.Domain;
using BeProductive.Modules.Goals.Domain;
using BeProductive.Modules.Goals.Domain.Entities;

public class GoalDayState : GoalExtensionBaseEntity
{
    public DateOnly Day { get; set; }
    public GoalState State { get; set; }
}