using System.Text.Json;
using Blazored.LocalStorage;
using Dapper;

namespace BeProductive.Modules.Settings.Infrastructure;

public class SettingsService
{
    private const string SettingsStorageKey = "settings";
    
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<SettingsService> _logger;

    public SettingsService(
        ILocalStorageService localStorage,
        ILogger<SettingsService> logger)
    {
        _localStorage = localStorage;
        _logger = logger;
    }

    public bool IsDarkTheme { get; private set; }

    public string Theme => IsDarkTheme ? "dark" : "light";

    public event EventHandler<string>? ThemeChanged;

    public void SetDarkTheme(bool isDark)
    {
        _logger.LogInformation("Changed dark mode to {@DarkMode}", isDark);
        IsDarkTheme = isDark;
        ThemeChanged?.Invoke(this, Theme);
        _ = SaveSettings();
    }

    public async ValueTask SaveSettings()
    {
        _logger.LogInformation("Saving settings to local storage");

        var settings = new Domain.Settings()
        {
            DarkThemeEnabled = IsDarkTheme,
        };

        await _localStorage.SetItemAsync(SettingsStorageKey, settings);
    }

    public async ValueTask LoadSettings()
    {
        _logger.LogInformation("Loading settings from local storage");
        
        var settings = await _localStorage.GetItemAsync<Domain.Settings?>(SettingsStorageKey);
        if (settings is not null)
        {
            _logger.LogDebug("Settings loaded: {@Settings}", settings);
            
            if (IsDarkTheme != settings.DarkThemeEnabled)
            {
                IsDarkTheme = settings.DarkThemeEnabled;
                ThemeChanged?.Invoke(this, Theme);
            }
        }
    }
}