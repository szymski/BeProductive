using BeProductive.Modules.Common.Domain;
using BeProductive.Modules.Goals.Domain;

public class GoalDayState : BaseEntity
{
    public Goal Goal { get; set; }
    public DateOnly Day { get; set; }
    public GoalState State { get; set; }
}