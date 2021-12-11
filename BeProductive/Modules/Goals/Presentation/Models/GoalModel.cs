using System.ComponentModel.DataAnnotations;
using BeProductive.Modules.Goals.Domain;
using Bogus;
using OneOf.Types;

namespace BeProductive.Modules.Goals.Presentation.Models;

public class GoalModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Color { get; set; } = GoalColors.RandomColor;

    public string Icon { get; set; }

    [Required]
    [Range(0, (int)AllowedDaysOfWeek.All, ErrorMessage = "At least one day must be allowed")]
    public AllowedDaysOfWeek AllowedDaysOfWeek { get; set; } = AllowedDaysOfWeek.All;

    public static GoalModel FromGoal(Goal goal) => new()
    {
        Name = goal.Name,
        Color = goal.Color,
        Icon = goal.Icon,
        AllowedDaysOfWeek = goal.AllowedDaysOfWeek,
    };

    public Goal ApplyToGoal(Goal goal)
    {
        goal.Name = Name;
        goal.Color = Color;
        goal.Icon = Icon;
        goal.AllowedDaysOfWeek = AllowedDaysOfWeek; 
        goal.Validate();
        return goal;
    }
}