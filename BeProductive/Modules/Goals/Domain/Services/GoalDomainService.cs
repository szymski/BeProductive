using BeProductive.Modules.Users.Infrastructure;
using BeProductive.Modules.Users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Domain.Services;

public class GoalDomainService
{
    private readonly IDbContextFactory<AppContext> _contextFactory;
    private readonly AuthService _authService;
    private readonly ILogger<GoalDomainService> _logger;

    protected int UserId => _authService.GetAuthData()!.UserId;

    public GoalDomainService(
        ILogger<GoalDomainService> logger,
        IDbContextFactory<AppContext> contextFactory,
        AuthService authService)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _authService = authService;
    }

    public async Task<bool> SetStateForDay(Goal goal, DateOnly day, GoalState state)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var goalEntry = await context.Goals
            .Where(x => x.UserId == UserId)
            .SingleAsync(x => x.Id == goal.Id);

        _logger.LogInformation("Setting goal {@Goal} state for day {Day} to {State}", goal, day, state);

        var entry = await context.Entry(goalEntry)
            .Collection(goal => goal.GoalDayStates)
            .Query()
            .SingleOrDefaultAsync(goal => goal.Day == day);

        if (state == GoalState.Unknown)
        {
            if (entry is null) return false;

            context.GoalDayStates.Remove(entry);
        }
        else
        {
            if (day.ToDateTime(TimeOnly.MinValue) >= DateTime.Today.AddDays(1))
            {
                _logger.LogWarning("Attempted to change state to future date ({Date}) for goal {@Goal}", day, goal);
                throw new ArgumentException("Cannot set state for future days");
            }

            if (entry?.State == state)
            {
                return false;
            }

            entry ??= new()
            {
                Goal = goalEntry,
                Day = day,
                State = state,
            };

            entry.State = state;

            context.GoalDayStates.Update(entry);
        }

        await context.SaveChangesAsync();

        return true;
    }
}