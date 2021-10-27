using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Infrastructure;

public static class CommonModuleExtensions
{
    public static IServiceCollection AddCommonModule(this IServiceCollection services)
    {
        services.AddDbContextFactory<AppContext>(options =>
        {
            options.UseSqlite($"Data Source=database.db");
        });
        return services;
    }
}