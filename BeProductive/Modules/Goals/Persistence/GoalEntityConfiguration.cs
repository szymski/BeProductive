using BeProductive.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GoalEntityConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.HasOne(goal => goal.User)
            .WithMany(user => user.Goals)
            .HasForeignKey(goal => goal.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(goal => goal.GoalDayStates)
            .WithOne(goalDayState => goalDayState.Goal);
    }
}