using BeProductive.Modules.Common.Domain;

namespace BeProductive.Modules.Goals.Domain.Entities;

public abstract class GoalExtensionBaseEntity : BaseEntity
{
    public int GoalId { get; set; }
    public Goal Goal { get; set; }
}