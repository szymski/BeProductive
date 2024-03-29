﻿using BeProductive.Modules.Common.Helpers;
using BeProductive.Modules.Rewards.Domain.Services;
using BeProductive.Modules.Users.Infrastructure;
using BeProductive.Modules.Users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Goals.Domain.Services;

public class GoalDomainService
{
    private readonly IDbContextFactory<AppContext> _contextFactory;
    private readonly AuthService _authService;
    private readonly ILogger<GoalDomainService> _logger;
    private readonly PointsDomainService _pointsService;

    protected int UserId => _authService.GetAuthData()!.UserId;

    public GoalDomainService(
        ILogger<GoalDomainService> logger,
        IDbContextFactory<AppContext> contextFactory,
        AuthService authService,
        PointsDomainService pointsService)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _authService = authService;
        _pointsService = pointsService;
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
            entry = null;
        }
        else
        {
            // Check for future date state change
            if (day.ToDateTime(TimeOnly.MinValue) >= DateTime.Today.AddDays(1))
            {
                _logger.LogWarning("Attempted to change state to future date ({Date}) for goal {@Goal}", day, goal);
                throw new ArgumentException("Cannot set state for future days");
            }

            if (entry?.State == state)
            {
                return false;
            }
            
            // Check if allowed day
            if (!DateHelper.IsDayAllowed(goal.AllowedDaysOfWeek, day))
            {
                _logger.LogWarning("Attempted to change state for disallowed date {Date} for goal {@Goal}", day, goal);
                throw new ArgumentException("Cannot set state for disallowed days");
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

        await _pointsService.AddGoalDayStatePoints(entry);

        return true;
    }
}