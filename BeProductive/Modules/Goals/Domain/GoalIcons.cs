namespace BeProductive.Modules.Goals.Domain;

public static class GoalIcons
{
    public static string[] Icons = new[]
    {
        "home",
        "setting",
        "smile",
        "user",
        "team",
        "form",
        "gitlab",
        "youtube",
        "bell",
        "calendar",
        "bulb",
        "cloud",
        "bank",
        "dollar",
        "experiment",
        "eye",
        "file",
        "global",
        "inbox",
        "laptop",
        "message",
        "phone",
        "skin",
        "shop",
        "tool",
        "star",
        "tablet",
        "schedule",
    };
    
    public static string RandomIcon => Icons[Random.Shared.Next(Icons.Length)];
}