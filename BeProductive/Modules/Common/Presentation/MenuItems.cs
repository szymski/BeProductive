using AntDesign;

namespace BeProductive.Modules.Common.Presentation; 

public record MyMenuItem(string Name, string Route, string? Icon = null);

public static class MenuItems {
    public static readonly List<MyMenuItem> Items = new()
    {
        new("Goals", "/", IconType.Outline.UnorderedList),
        new("Rituals", "/rituals", IconType.Outline.Bell),
        new("Timer", "/timer", IconType.Fill.ClockCircle),
        new("To do", "/todo", IconType.Outline.Check),
        new("Components", "/components", IconType.Fill.Layout),
        new("Account", "/account", IconType.Outline.User),
    };   
}