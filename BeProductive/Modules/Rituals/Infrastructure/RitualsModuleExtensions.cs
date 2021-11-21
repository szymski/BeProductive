namespace BeProductive.Modules.Rituals.Infrastructure;

public static class RitualsModuleExtensions
{
    public static IServiceCollection AddRitualsModule(this IServiceCollection services)
    {
        services.AddScoped<RitualDomainService>();
        services.AddScoped<RitualGoalService>();
        return services;
    }
}
