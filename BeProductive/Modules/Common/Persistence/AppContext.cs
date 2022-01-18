using System.Reflection;
using BeProductive.Modules.GoalExtensions.Description.Domain;
using BeProductive.Modules.GoalExtensions.EmergencyGoal.Domain;
using BeProductive.Modules.Goals.Domain;
using BeProductive.Modules.Rewards.Domain;
using BeProductive.Modules.Rituals.Domain;
using BeProductive.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BeProductive.Modules.Common.Persistence;

public class AppContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Goal> Goals { get; set; }
    public DbSet<GoalDayState> GoalDayStates { get; set; }
    public DbSet<EmergencyGoal> EmergencyGoals { get; set; }
    public DbSet<GoalDescription> GoalDescriptions { get; set; }

    public DbSet<Ritual> Rituals { get; set; }
    
    public DbSet<PointClaimEvent> PointClaimEvents { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    
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

        // TODO: Move this to another file
        if (Database.IsNpgsql())
        {
            modelBuilder.HasPostgresEnum<GoalState>();
            modelBuilder.HasPostgresEnum<PointClaimSourceType>();
        }
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    static AppContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<GoalState>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<PointClaimSourceType>();
    }
    
    public static string Provider { get; set; } = "Uninitialized";
    public static bool IsPostgres => Provider == "Postgres";
}