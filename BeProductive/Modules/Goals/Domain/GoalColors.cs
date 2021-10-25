namespace BeProductive.Modules.Goals.Domain;

public static class GoalColors
{
    public static string[] Colors = new[]
    {
        "#cf1322",
        "#fa8c16",
        "#a0d911",
        "#13c2c2",
        "#1890ff",
        "#722ed1",
        "#eb2f96",
        "#fadb14",
    };

    public static string RandomColor => Colors[Random.Shared.Next(Colors.Length)];
}