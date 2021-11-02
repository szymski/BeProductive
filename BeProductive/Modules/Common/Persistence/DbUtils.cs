using System.Diagnostics;
using BeProductive.Modules.Goals.Domain;
using BeProductive.Modules.Rituals.Domain;
using BeProductive.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Persistence;

public static class DbUtils
{
    public static async Task InitializeDb(AppContext context, UserManager<User> userManager)
    {
        // var builder = new DbContextOptionsBuilder<AppContext>(options);
        // await using var context = new AppContext(builder.Options);

        await context.Database.EnsureCreatedAsync();
        SeedDb(context, userManager);
    }

    private static void SeedDb(AppContext context, UserManager<User> userManager)
    {
        SeedGoals(context);
        SeedRituals(context);
        SeedUsers(context, userManager);

        context.SaveChanges();
    }

    private static void SeedGoals(AppContext context)
    {
        if (context.Goals.Any()) return;

        Console.WriteLine("Seeding Goals");

        context.Goals.AddRange(new Goal[]
        {
            new() { Name = "I am from database!", Color = "#ff22ff", Icon = "windows" },
            new() { Name = "Zwalić gruchę raz dziennie", Color = GoalColors.Colors[2], Icon = GoalIcons.Icons[2] },
            new() { Name = "Wykonać poranny rytuał", Color = GoalColors.Colors[3] },
            new() { Name = "Obrazić kogoś", Color = GoalColors.Colors[4], Icon = GoalIcons.Icons[8] },
            new() { Name = "Dupa", Color = GoalColors.Colors[5] },
        });
    }

    private static void SeedRituals(AppContext context)
    {
        if (context.Rituals.Any()) return;

        Console.WriteLine("Seeding rituals");

        context.Rituals.AddRange(new Ritual[]
        {
            new() { Type = RitualType.Morning, Title = "Zrobić 15 przysiadów", Order = 0 },
            new() { Type = RitualType.Morning, Title = "Umyć zęby", Order = 1 },
            new() { Type = RitualType.Morning, Title = "Zagrać w Beat Saber", Order = 2 },
            new() { Type = RitualType.Evening, Title = "Wyłączyć komputer", Order = 0 },
            new() { Type = RitualType.Evening, Title = "Zgasić światła", Order = 1 },
            new() { Type = RitualType.Evening, Title = "Pożegnać Natalkę", Order = 2 },
        });
    }

    private static void SeedUsers(AppContext context, UserManager<User> userManager)
    {
        if(context.Users.Any()) return;

        Console.WriteLine("Seeding users");
        
        var task = userManager.CreateAsync(new ()
        {
            UserName = "user",
            FullName = "Test User",

        }, "pwd123");
        task.Wait();

        var result = task.Result;

        if (result.Succeeded)
        {
            Console.WriteLine("Seeding user succeeded");
        }
        else
        {
            Console.Error.WriteLine("Seeding user failed");
            Console.Error.WriteLine(result.Errors.Aggregate("", (acc, err) => acc + err.Description + "\n"));
        }
    }
}