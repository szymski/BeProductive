namespace BeProductive.Modules.Settings.Infrastructure;

public static class SettingsModuleExtensions
{
    public static IServiceCollection AddSettingsModule(this IServiceCollection services)
    {
        services.AddScoped<SettingsService>();
        
        return services;
    }
}