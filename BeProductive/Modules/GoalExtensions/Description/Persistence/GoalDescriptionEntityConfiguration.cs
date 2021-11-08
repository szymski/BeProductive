using BeProductive.Modules.GoalExtensions.Description.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeProductive.Modules.GoalExtensions.Description.Persistence;

public class GoalDescriptionEntityConfiguration :
    IEntityTypeConfiguration<GoalDescription>,
    IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<GoalDescription> builder)
    {
        builder
            .HasOne(x => x.Goal)
            .WithOne(x => x.GoalDescription)
            .HasForeignKey<GoalDescription>(x => x.GoalId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder
            .Navigation(goal => goal.GoalDescription)
            .AutoInclude();
    }
}