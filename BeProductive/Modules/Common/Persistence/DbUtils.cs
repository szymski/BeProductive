using System.Diagnostics;
using BeProductive.Modules.Goals.Domain;
using BeProductive.Modules.Rituals.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Persistence;

public static class DbUtils
{
    public static async Task InitializeDb(AppContext context)
    {
        // var builder = new DbContextOptionsBuilder<AppContext>(options);
        // await using var context = new AppContext(builder.Options);

        await context.Database.EnsureCreatedAsync();
        SeedDb(context);
    }

    private static void SeedDb(AppContext context)
    {
        SeedGoals(context);
        SeedRituals(context);

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
}