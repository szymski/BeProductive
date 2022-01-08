using Blazored.LocalStorage;

namespace BeProductive.Modules.Settings.Infrastructure;

public class SettingsService {
    private const string SettingsStorageKey = "settings";
    private const string ThemeCookieName = "theme";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<SettingsService> _logger;

    public SettingsService(
        ILocalStorageService localStorage,
        ILogger<SettingsService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _localStorage = localStorage;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsDarkTheme { get; private set; }

    public string Theme => IsDarkTheme ? "dark" : "light";

    public bool SoundsEnabled { get; private set; } = true;

    public event EventHandler<string>? ThemeChanged;

    public void SetDarkTheme(bool isDark)
    {
        _logger.LogDebug("Changed dark mode to {@DarkMode}", isDark);
        IsDarkTheme = isDark;
        ThemeChanged?.Invoke(this, Theme);
        _ = SaveSettings();
    }

    public void SetSoundsEnabled(bool enabled)
    {
        _logger.LogInformation("Settings sounds enabled to {@SoundsEnabled}", enabled);
        SoundsEnabled = enabled;
        _ = SaveSettings();
    }

    public async ValueTask SaveSettings()
    {
        _logger.LogInformation("Saving settings to local storage");

        var settings = new Domain.Settings()
        {
            DarkThemeEnabled = IsDarkTheme,
            SoundsEnabled = SoundsEnabled,
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

            SoundsEnabled = settings.SoundsEnabled;
        }
        
        // TODO: Save theme to cookie - like AuthService
    }

    public void LoadCookiesSync()
    {
        if (_httpContextAccessor?.HttpContext?.Request.Cookies is {} cookies)
        {
            if (cookies.TryGetValue(ThemeCookieName, out var theme))
            {
                IsDarkTheme = theme == "dark";
                ThemeChanged?.Invoke(this, Theme);
                _logger.LogWarning("Loaded theme from cookies: {Theme}", cookies[ThemeCookieName]);
            }
        }
    }
}