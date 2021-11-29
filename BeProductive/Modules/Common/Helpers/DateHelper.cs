using BeProductive.Modules.Goals.Domain;

namespace BeProductive.Modules.Common.Helpers;

public static class DateHelper
{
    public static (DateTime firstDay, DateTime lastDay) FirstAndLastDayOfMonth(DateTime date) =>
    (
        new DateTime(date.Year, date.Month, 1),
        new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month))
    );

    public static (DateOnly firstDay, DateOnly lastDay) FirstAndLastDayOfMonth(DateOnly date) =>
    (
        new DateOnly(date.Year, date.Month, 1),
        new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month))
    );

    public static bool IsSameDay(DateOnly date1, DateOnly date2) =>
        AntDesign.DateHelper.IsSameDay(
            date1.ToDateTime(TimeOnly.MinValue),
            date2.ToDateTime(TimeOnly.MinValue)
        );

    public static bool IsDayAllowed(AllowedDaysOfWeek allowed, DateOnly day) =>
        allowed.HasFlag(DayOfWeekToAllowedDay(day.DayOfWeek));
    
    public static AllowedDaysOfWeek ToAllowedDayOfWeek(DateTime date) =>
        DayOfWeekToAllowedDay(date.DayOfWeek);

    private static AllowedDaysOfWeek DayOfWeekToAllowedDay(DayOfWeek date) =>
        date switch
        {
            DayOfWeek.Monday => AllowedDaysOfWeek.Monday,
            DayOfWeek.Tuesday => AllowedDaysOfWeek.Tuesday,
            DayOfWeek.Wednesday => AllowedDaysOfWeek.Wednesday,
            DayOfWeek.Thursday => AllowedDaysOfWeek.Thursday,
            DayOfWeek.Friday => AllowedDaysOfWeek.Friday,
            DayOfWeek.Saturday => AllowedDaysOfWeek.Saturday,
            DayOfWeek.Sunday => AllowedDaysOfWeek.Sunday,
            _ => throw new ArgumentOutOfRangeException(nameof(date), date, "Invalid day of week"),
        };
}