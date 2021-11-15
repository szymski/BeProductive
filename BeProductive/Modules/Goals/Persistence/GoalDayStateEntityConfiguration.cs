using BeProductive.Modules.Goals.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GoalDayStateEntityConfiguration : IEntityTypeConfiguration<GoalDayState>
{
    public void Configure(EntityTypeBuilder<GoalDayState> builder)
    {
        builder.HasOne(goalDayState => goalDayState.Goal)
            .WithMany(goal => goal.GoalDayStates)
            .HasForeignKey(goalDayState => goalDayState.GoalId)
            .OnDelete(DeleteBehavior.Cascade);

        if (AppContext.IsPostgres)
        {
            builder.Property(datState => datState.State)
                .HasConversion<GoalState>();
        }
        else
        {
            builder.Property(datState => datState.State)
                .HasConversion<string>();
        }
    }
}