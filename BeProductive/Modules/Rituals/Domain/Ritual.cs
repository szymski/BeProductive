using BeProductive.Modules.Common.Domain;

namespace BeProductive.Modules.Rituals.Domain;

public class Ritual : BaseEntity
{
    public string Title { get; set; }
    public RitualType Type { get; set; }
    public int Order { get; set; }
}