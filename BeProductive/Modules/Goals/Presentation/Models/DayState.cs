using BeProductive.Modules.Goals.Domain;

namespace BeProductive.Modules.Goals.Presentation.Models;

public record DayState(DateOnly Day, GoalState State)
{
    public DayState(DateTime day, GoalState state) : this(DateOnly.FromDateTime(day), state)
    {
    }

    public static DayState FromDomain(GoalDayState dayState)
        => new(dayState.Day, dayState.State);
};