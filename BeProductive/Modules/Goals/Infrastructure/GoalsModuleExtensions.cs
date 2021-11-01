using BeProductive.Modules.Goals.Domain.Services;

namespace BeProductive.Modules.Goals.Infrastructure;

public static class GoalsModuleExtensions
{
    public static IServiceCollection AddGoalsModule(this IServiceCollection services)
    {
        services.AddSingleton<GoalService>();
        services.AddSingleton<GoalDomainService>();
        return services;
    }
}
