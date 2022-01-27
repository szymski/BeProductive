using BeProductive.Modules.Goals.Domain;
using BeProductive.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Rewards.Domain.Services;

public class PointsDomainService {
    private readonly ILogger<PointsDomainService> _logger;
    private readonly IDbContextFactory<AppContext> _contextFactory;

    public PointsDomainService(
        IDbContextFactory<AppContext> contextFactory,
        ILogger<PointsDomainService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public event Action<int>? OnPointsChanged;

    public async Task<int> GetPointBalance(int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var lastEvent = await context.PointClaimEvents
            .Where(ev => ev.UserId == userId)
            .OrderByDescending(ev => ev.ClaimedAt)
            .FirstOrDefaultAsync();

        var balance = lastEvent?.TotalBalance ?? 0;

        return balance;
    }

    public async Task<int> CalculateTotalBalance(int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var balance = await context.PointClaimEvents
            .Where(ev => ev.UserId == userId)
            .SumAsync(ev => ev.PointsClaimed);

        return balance;
    }

    public async Task<PointClaimEvent?> AddGoalDayStatePoints(GoalDayState? dayState)
    {
        if (dayState == null)
        {
            return null;
        }

        var pointsToAdd = dayState.State switch
        {
            GoalState.Success => 1,
            GoalState.Failure => -1,
            _ => 0,
        };

        if (pointsToAdd == 0)
        {
            return null;
        }

        await using var context = await _contextFactory.CreateDbContextAsync();

        dayState = await context.GoalDayStates
            .Include(state => state.Goal)
            .SingleAsync(state => state.Id == dayState.Id);

        // Find existing event for this day state
        var existingEvent = await context.PointClaimEvents
            .Where(ev => ev.UserId == dayState.Goal.UserId)
            .Where(ev => ev.PointClaimSourceType == PointClaimSourceType.GoalDayState)
            .Where(ev => ev.SourceGoalId == dayState.GoalId)
            .Where(ev => ev.SourceDay == dayState.Day)
            .FirstOrDefaultAsync();
        if (existingEvent is not null)
        {
            _logger.LogWarning("Attempted to add points for a day which already has an event {@Event}", existingEvent);
            return null;
        }

        var newEvent = new PointClaimEvent
        {
            UserId = dayState.Goal.UserId,
            PointsClaimed = pointsToAdd,
            TotalBalance = await CalculateTotalBalance(dayState.Goal.UserId) + pointsToAdd,
            PointClaimSourceType = PointClaimSourceType.GoalDayState,
            SourceGoalId = dayState.GoalId,
            SourceDay = dayState.Day,
        };

        _logger.LogInformation("Adding {Points} points for goal day state at {Date}", pointsToAdd, dayState.Day);

        context.PointClaimEvents.Add(newEvent);
        await context.SaveChangesAsync();
        
        OnPointsChanged?.Invoke(newEvent.TotalBalance);

        return newEvent;
    }

    public async Task<PointClaimEvent?> AddSeedPoints(int userId, int points)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var newEvent = new PointClaimEvent
        {
            UserId = userId,
            PointsClaimed = points,
            TotalBalance = await CalculateTotalBalance(userId) + points,
            PointClaimSourceType = PointClaimSourceType.Seed,
        };

        context.PointClaimEvents.Add(newEvent);
        await context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Points} points for user id {UserId}", points, userId);
        
        OnPointsChanged?.Invoke(newEvent.TotalBalance);

        return newEvent;
    }
}