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
}