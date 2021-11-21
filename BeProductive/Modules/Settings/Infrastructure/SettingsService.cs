using Serilog;

namespace BeProductive.Modules.Settings.Infrastructure;

public class SettingsService
{
    private ILogger<SettingsService> _logger;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
    }

    public bool IsDarkTheme { get; private set; }

    public string Theme => IsDarkTheme ? "dark" : "light";

    public event EventHandler<string> ThemeChanged;

    public void SetDarkTheme(bool isDark)
    {
        Log.Information("Changed dark mode to {@DarkMode}", IsDarkTheme);
        IsDarkTheme = isDark;
        ThemeChanged?.Invoke(this, Theme);
    }

}