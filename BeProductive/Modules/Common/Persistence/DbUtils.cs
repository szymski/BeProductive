using System.Diagnostics;
using BeProductive.Modules.Goals.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Persistence;

public static class DbUtils
{
    public static async Task InitializeDb(AppContext context)
    {
        // var builder = new DbContextOptionsBuilder<AppContext>(options);
        // await using var context = new AppContext(builder.Options);

        if (await context.Database.EnsureCreatedAsync())
        {
            SeedDb(context);
        }
    }

    private static void SeedDb(AppContext context)
    {
        var isInitialized = context.Goals.Any();
        if (isInitialized)
        {
            return;
        }
        
        Debug.WriteLine("Seeding database");
        
        context.Goals.AddRange(new Goal []
        {
            new() { Name = "I am from database!", Color = "#ff22ff", Icon = "windows" },
            new() { Name = "Zwalić gruchę raz dziennie", Color = GoalColors.Colors[2], Icon = GoalIcons.Icons[2] },
            new() { Name = "Wykonać poranny rytuał", Color = GoalColors.Colors[3] },
            new() { Name = "Obrazić kogoś", Color = GoalColors.Colors[4], Icon = GoalIcons.Icons[8] },
            new() { Name = "Dupa", Color = GoalColors.Colors[5] },
        });

        context.SaveChanges();
    }
}