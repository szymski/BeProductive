using BeProductive.Modules.Rewards.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeProductive.Modules.Rewards.Persistence; 

public class PointClaimEventEntityConfiguration : IEntityTypeConfiguration<PointClaimEvent> {

    public void Configure(EntityTypeBuilder<PointClaimEvent> builder)
    {
        builder.HasOne(ev => ev.User)
            .WithMany(user => user.PointEvents)
            .HasForeignKey(ev => ev.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}