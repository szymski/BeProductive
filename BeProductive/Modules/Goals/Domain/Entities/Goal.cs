using BeProductive.Modules.Common.Domain;

public class Goal : BaseEntity
{
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; }
    
    public List<GoalDayState> GoalDayStates { get; set; }

    public override string ToString()
    {
        return $"Goal {{ {nameof(Id)} = {Id} }}";
    }
}