using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GoalDayStateEntityConfiguration : IEntityTypeConfiguration<GoalDayState>
{
    public void Configure(EntityTypeBuilder<GoalDayState> builder)
    {
        builder.HasOne<Goal>(goalDayState => goalDayState.Goal)
            .WithMany(goal => goal.GoalDayStates)
            .OnDelete(DeleteBehavior.Cascade);
    }
}