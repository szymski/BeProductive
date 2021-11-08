using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeProductive.Modules.GoalExtensions.EmergencyGoal.Persistence;

public class EmergencyGoalEntityConfiguration :
    IEntityTypeConfiguration<Domain.EmergencyGoal>,
    IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Domain.EmergencyGoal> builder)
    {
        builder
            .HasOne(x => x.Goal)
            .WithOne(x => x.EmergencyGoal)
            .HasForeignKey<Domain.EmergencyGoal>(x => x.GoalId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder
            .Navigation(goal => goal.EmergencyGoal)
            .AutoInclude();
    }
}