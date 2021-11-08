using BeProductive.Modules.GoalExtensions.EmergencyGoal.Infrastructure;
using BeProductive.Modules.Goals.Domain.Services;

namespace BeProductive.Modules.Goals.Infrastructure;

public static class GoalsModuleExtensions
{
    public static IServiceCollection AddGoalsModule(this IServiceCollection services)
    {
        services.AddScoped<GoalService>();
        services.AddScoped<GoalDomainService>();
        services.AddScoped<EmergencyGoalService>();
        return services;
    }
}
