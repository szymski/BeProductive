using BeProductive.Modules.Rituals.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeProductive.Modules.Rituals.Persistence;

public class RitualEntityConfiguration : IEntityTypeConfiguration<Ritual>
{
    public void Configure(EntityTypeBuilder<Ritual> builder)
    {
        builder.HasOne(ritual => ritual.User)
            .WithMany(goal => goal.Rituals)
            .HasForeignKey(ritual => ritual.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}