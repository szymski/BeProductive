using BeProductive.Modules.Common.Domain;

public class Goal : BaseEntity
{
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Icon { get; set; }
    
    public List<GoalDayState> GoalDayStates { get; set; } 
}