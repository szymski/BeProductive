using BeProductive.Modules.Rituals.Domain;
using BeProductive.Modules.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Rituals.Infrastructure;

public class RitualDomainService
{
    private readonly AppContext _context;
    private readonly AuthService _authService;
    private readonly ILogger<RitualDomainService> _logger;

    protected int UserId => _authService.GetAuthData()!.UserId;

    public RitualDomainService(
        AppContext context,
        ILogger<RitualDomainService> logger,
        AuthService authService)
    {
        _context = context;
        _logger = logger;
        _authService = authService;
    }

    public async Task<IReadOnlyList<Ritual>> GetRituals(RitualType type)
    {
        return await _context.Rituals
            .Where(ritual => ritual.UserId == UserId)
            .Where(ritual => ritual.Type == type)
            .OrderBy(ritual => ritual.Order)
            .ToListAsync();
    }

    public async Task<Ritual> AddRitual(Ritual ritual)
    {
        ritual.UserId = UserId;

        await _context.Rituals.AddAsync(ritual);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Added ritual {@Ritual}", ritual);
        return ritual;
    }

    public async Task<Ritual> UpdateRitual(Ritual ritual)
    {
        _logger.LogInformation("Updating ritual {@Ritual}", ritual);
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
        _logger.LogDebug("Updating ritual orders for {@Rituals} to {@Orders}",
            ritualOrders.Select(x => x.Key),
            ritualOrders.Select(x => x.Value));

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