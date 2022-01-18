using BeProductive.Modules.GoalExtensions.Description.Infrastructure;
using BeProductive.Modules.GoalExtensions.EmergencyGoal.Infrastructure;
using BeProductive.Modules.Goals.Domain.Services;
using BeProductive.Modules.Rewards.Domain.Services;

namespace BeProductive.Modules.Rewards.Infrastructure;

public static class RewardsModuleExtensions
{
    public static IServiceCollection AddRewardsModule(this IServiceCollection services)
    {
        services.AddScoped<PointsDomainService>();
        return services;
    }
}
