using BeProductive.Modules.Rituals.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Rituals.Infrastructure;

public class RitualDomainService
{
    private readonly AppContext _context;

    public RitualDomainService(AppContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Ritual>> GetRituals(RitualType type)
    {
        return await _context.Rituals
            .Where(ritual => ritual.Type == type)
            .OrderBy(ritual => ritual.Order)
            .ToListAsync();
    }

    public async Task<Ritual> AddRitual(Ritual ritual)
    {
        await _context.Rituals.AddAsync(ritual);
        await _context.SaveChangesAsync();
        return ritual;
    }

    public async Task<Ritual> UpdateRitual(Ritual ritual)
    {
        _context.Rituals.Update(ritual);
        await _context.SaveChangesAsync();
        return ritual;
    }

    public async Task RemoveRitual(Ritual ritual)
    {
        _context.Rituals.Remove(ritual);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrders(IEnumerable<KeyValuePair<Ritual, int>> ritualOrders)
    {
        var ritualIds = ritualOrders.Select(x => x.Key);
        
        var rituals = await _context.Rituals
            .Where(ritual => ritualIds.Contains(ritual))
            .ToArrayAsync();

        foreach (var (ritual, order) in ritualOrders)
        {
            var entity = rituals.Single(r => r.Id == ritual.Id);
            entity.Order = order;
        }

        await _context.SaveChangesAsync();
    }
}