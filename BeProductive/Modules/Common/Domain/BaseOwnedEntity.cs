using BeProductive.Modules.Users.Domain;

namespace BeProductive.Modules.Common.Domain;

public abstract class BaseOwnedEntity : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
}