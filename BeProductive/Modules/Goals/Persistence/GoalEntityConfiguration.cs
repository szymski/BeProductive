using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GoalEntityConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.HasMany(goal => goal.GoalDayStates)
            .WithOne(goalDayState => goalDayState.Goal);
    }
}