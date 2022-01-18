using BeProductive.Modules.Common.Domain;

namespace BeProductive.Modules.Rewards.Domain; 

public class PointClaimEvent : BaseOwnedEntity {
    public int PointsClaimed { get; set; }
    public int TotalBalance { get; set; }

    public DateTime ClaimedAt { get; set; } = DateTime.UtcNow;

    public PointClaimSourceType PointClaimSourceType { get; set; }
    public int? SourceGoalId { get; set; }
    public DateOnly? SourceDay { get; set; }

}