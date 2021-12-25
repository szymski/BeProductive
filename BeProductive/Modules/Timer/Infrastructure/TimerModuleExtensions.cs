namespace BeProductive.Modules.Timer.Infrastructure;

public static class TimerModuleExtensions
{
    public static IServiceCollection AddTimerModule(this IServiceCollection services)
    {
        services.AddScoped<TimerService>();
        
        return services;
    }
} 