using BeProductive.Modules.Common.Domain;

namespace BeProductive.Modules.Rewards.Domain; 

public class Reward : BaseOwnedEntity {
    public string Name { get; set; }
    public int Price { get; set; }
    public bool IsSingleUse { get; set; } = false;
}