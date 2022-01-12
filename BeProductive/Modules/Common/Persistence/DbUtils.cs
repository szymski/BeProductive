using System.Diagnostics;
using BeProductive.Modules.Goals.Domain;
using BeProductive.Modules.Rituals.Domain;
using BeProductive.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BeProductive.Modules.Common.Persistence;

public static class DbUtils
{
    public static async Task InitializeDb(AppContext context, UserManager<User> userManager, bool isDevelopment)
    {
        CreateSchema(context);
        if (isDevelopment)
        {
            Log.Information("Development environment - Creating database without migrations...");
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            Log.Information("Production environment - Applying migrations...");
            await context.Database.MigrateAsync();
        }
        SeedDb(context, userManager);
    }

    private static void SeedDb(AppContext context, UserManager<User> userManager)
    {
        SeedUsers(context, userManager);
        SeedGoals(context);
        SeedRituals(context);

        context.SaveChanges();
    }

    private static void CreateSchema(AppContext context)
    {
        if (!context.Database.IsNpgsql()) return;

        Log.Information("Creating schema");
        context.Database.ExecuteSqlRaw("CREATE SCHEMA IF NOT EXISTS public");
    }

    private static void SeedGoals(AppContext context)
    {
        if (context.Goals.Any()) return;

        Log.Information("Seeding Goals");

        var user = context.Users.First();

        context.Goals.AddRange(new Goal[]
        {
            new() { User = user, Name = "I am from database!", Color = "#ff22ff", Icon = "windows" },
            new() { User = user, Name = "Zwalić gruchę raz dziennie", Color = GoalColors.Colors[2], Icon = GoalIcons.Icons[2] },
            new() { User = user, Name = "Wykonać poranny rytuał", Color = GoalColors.Colors[3] },
            new() { User = user, Name = "Obrazić kogoś", Color = GoalColors.Colors[4], Icon = GoalIcons.Icons[8] },
            new() { User = user, Name = "Dupa", Color = GoalColors.Colors[5] },
        });
    }

    private static void SeedRituals(AppContext context)
    {
        if (context.Rituals.Any()) return;

        Log.Information("Seeding rituals");
        
        var user = context.Users.First();

        context.Rituals.AddRange(new Ritual[]
        {
            new() { UserId = user.Id, Type = RitualType.Morning, Title = "Zrobić 15 przysiadów", Order = 0 },
            new() { UserId = user.Id, Type = RitualType.Morning, Title = "Umyć zęby", Order = 1 },
            new() { UserId = user.Id, Type = RitualType.Morning, Title = "Zagrać w Beat Saber", Order = 2 },
            new() { UserId = user.Id, Type = RitualType.Evening, Title = "Wyłączyć komputer", Order = 0 },
            new() { UserId = user.Id, Type = RitualType.Evening, Title = "Zgasić światła", Order = 1 },
            new() { UserId = user.Id, Type = RitualType.Evening, Title = "Pożegnać Natalkę", Order = 2 },
        });
    }

    private static void SeedUsers(AppContext context, UserManager<User> userManager)
    {
        if(context.Users.Any()) return;

        Log.Information("Seeding users");

        {
            var task = userManager.CreateAsync(new()
            {
                UserName = "user",
                FullName = "Test User",

            }, "pwd123");
            task.Wait();

            var result = task.Result;

            if (result.Succeeded)
            {
                Log.Information("Seeding user succeeded");
            }
            else
            {
                Console.Error.WriteLine("Seeding user failed");
                Console.Error.WriteLine(result.Errors.Aggregate("", (acc, err) => acc + err.Description + "\n"));
            }
        }
        
        {
            var task = userManager.CreateAsync(new()
            {
                UserName = "user2",
                FullName = "Test user 2",

            }, "1234");
            task.Wait();

            var result = task.Result;

            if (result.Succeeded)
            {
                Log.Information("Seeding user 2 succeeded");
            }
            else
            {
                Console.Error.WriteLine("Seeding user 2 failed");
                Console.Error.WriteLine(result.Errors.Aggregate("", (acc, err) => acc + err.Description + "\n"));
            }
        }
    }
}