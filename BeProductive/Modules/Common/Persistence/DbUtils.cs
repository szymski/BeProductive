using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Persistence;

public static class DbUtils
{
    public static async Task InitializeDb(DbContextOptions<AppContext> options)
    {
        var builder = new DbContextOptionsBuilder<AppContext>(options);
        await using var context = new AppContext(builder.Options);

        if (await context.Database.EnsureCreatedAsync())
        {
            SeedDb(context);
        }
    }

    private static void SeedDb(AppContext context)
    {
        
    }
}