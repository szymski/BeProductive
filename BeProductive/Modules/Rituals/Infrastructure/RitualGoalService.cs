using BeProductive.Modules.Goals.Domain;
using BeProductive.Modules.Goals.Infrastructure;
using BeProductive.Modules.Rituals.Domain;
using BeProductive.Modules.Users.Infrastructure;

namespace BeProductive.Modules.Rituals.Infrastructure;

public class RitualGoalService
{
    private readonly AppContext _context;
    private readonly AuthService _authService;
    private readonly ILogger<RitualGoalService> _logger;
    private readonly GoalService _goalService;

    protected int UserId => _authService.GetAuthData()!.UserId;

    public RitualGoalService(
        AppContext context,
        AuthService authService,
        ILogger<RitualGoalService> logger,
        GoalService goalService)
    {
        _context = context;
        _authService = authService;
        _logger = logger;
        _goalService = goalService;
    }

    public async Task AddRitualGoals()
    {
        var (hasMorning, hasEvening) = await HasRitualGoals();

        if (!hasMorning)
        {
            await _goalService.AddSystemGoal(new Goal()
            {
                Name = "Perform morning ritual",
                Icon = "clock-circle",
                Color = "#e8c92b",
            }, RitualGoalTypes.MorningRitual);
        }

        if (!hasEvening)
        {
            await _goalService.AddSystemGoal(new Goal()
            {
                Name = "Perform evening ritual",
                Icon = "clock-circle",
                Color = "#2799e6",
            }, RitualGoalTypes.EveningRitual);
        }
    }

    public async Task<(bool morning, bool evening)> HasRitualGoals()
    {
        var types = new[] { RitualGoalTypes.MorningRitual, RitualGoalTypes.EveningRitual };
        var goals = await _goalService.GetSystemGoalsByTypes(types);

        return (goals.ContainsKey(RitualGoalTypes.MorningRitual), goals.ContainsKey(RitualGoalTypes.EveningRitual));
    }
}