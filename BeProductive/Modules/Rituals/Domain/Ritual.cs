using BeProductive.Modules.Common.Domain;

namespace BeProductive.Modules.Rituals.Domain;

public class Ritual : BaseOwnedEntity
{
    public string Title { get; set; }
    public RitualType Type { get; set; }
    public int Order { get; set; }

    public override string ToString()
    {
        return $"Ritual {{ Id = {Id}, UserId = {UserId}, Type = {Type}, Order = {Order}, Title = {Title} }}";
    }
}