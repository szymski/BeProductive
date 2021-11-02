using System.Reflection;
using BeProductive.Modules.Rituals.Domain;
using BeProductive.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Persistence;

public class AppContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Goal> Goals { get; set; }
    public DbSet<GoalDayState> GoalDayStates { get; set; }

    public DbSet<Ritual> Rituals { get; set; }

    public AppContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}