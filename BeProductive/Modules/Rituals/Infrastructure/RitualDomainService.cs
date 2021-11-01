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
}