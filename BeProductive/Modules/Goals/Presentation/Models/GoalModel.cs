using System.ComponentModel.DataAnnotations;
using BeProductive.Modules.Goals.Domain;

namespace BeProductive.Modules.Goals.Presentation.Models;

public class GoalModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Color { get; set; } = GoalColors.RandomColor;
    
    public string Icon { get; set; }

    public static GoalModel FromGoal(Goal goal) => new()
    {
        Name = goal.Name,
        Color = goal.Color,
        Icon = goal.Icon,
    };

    public Goal ApplyToGoal(Goal goal)
    {
        goal.Name = Name;
        goal.Color = Color;
        goal.Icon = Icon;
        return goal;
    }
}