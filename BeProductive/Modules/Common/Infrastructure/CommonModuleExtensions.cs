using BeProductive.Modules.Common.Presentation;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Infrastructure;

public static class CommonModuleExtensions
{
    public class Options
    {
        public string DbFile { get; set; }
    }
    
    public static IServiceCollection AddCommonModule(this IServiceCollection services, Action<Options> builder)
    {
        var options = new Options();
        builder(options);
        
        services.AddDbContext<AppContext>(dbOptions =>
        {
            dbOptions.UseSqlite($"Data Source={options.DbFile}");
        }, ServiceLifetime.Singleton);

        services.AddScoped<LayoutContext>();
        
        return services;
    }
}