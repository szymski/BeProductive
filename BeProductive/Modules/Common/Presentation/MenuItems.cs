namespace BeProductive.Modules.Common.Presentation; 

public record MenuItem(string Name, string Route);

public static class MenuItems {
    public static readonly List<MenuItem> Items = new()
    {
        new("Goals", "/"),
        new("Rituals", "/rituals"),
        new("Timer", "/timer"),
        new("To do", "/todo"),
        new("Components", "/components"),
        new("Account", "/account"),
    };   
}