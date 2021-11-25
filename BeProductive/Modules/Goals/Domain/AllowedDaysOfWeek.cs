namespace BeProductive.Modules.Goals.Domain;

[Flags]
public enum AllowedDaysOfWeek
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4,
    Thursday = 8,
    Friday = 16,
    Saturday = 32,
    Sunday = 64,

    All = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,
}