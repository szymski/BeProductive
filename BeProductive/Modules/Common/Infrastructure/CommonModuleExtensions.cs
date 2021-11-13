using BeProductive.Modules.Common.Presentation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BeProductive.Modules.Common.Infrastructure;

public static class CommonModuleExtensions
{
    public class Options
    {
        public DbOptions Database { get; set; }
    }

    public class DbOptions
    {
        public enum DbProvider
        {
            Sqlite,
            Postgres,
        }

        public DbProvider Provider { get; set; }
        public string ConnectionString { get; set; }
    }

    public static IServiceCollection AddCommonModule(this IServiceCollection services, Action<Options> builder)
    {
        var options = new Options();
        builder(options);

        // TODO: Dapper SQL logging
        services.AddDbContextFactory<AppContext>(dbOptions =>
        {
            Log.Information("Selected database provider {Provider} with connection string {ConnectionString}",
                options.Database.Provider, options.Database.ConnectionString);

            switch (options.Database.Provider)
            {
                case DbOptions.DbProvider.Sqlite:
                    dbOptions.UseSqlite(options.Database.ConnectionString);
                    break;
                case DbOptions.DbProvider.Postgres:
                    dbOptions.UseNpgsql(options.Database.ConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Database.Provider),
                        "Invalid database provider");
            }
        });

        services.AddScoped<LayoutContext>();

        return services;
    }
}