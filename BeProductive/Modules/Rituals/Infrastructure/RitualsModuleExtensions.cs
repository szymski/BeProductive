namespace BeProductive.Modules.Rituals.Infrastructure;

public static class RitualsModuleExtensions
{
    public static IServiceCollection AddRitualsModule(this IServiceCollection services)
    {
        services.AddSingleton<RitualDomainService>();
        return services;
    }
}
