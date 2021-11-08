using BeProductive.Modules.GoalExtensions.Description.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.GoalExtensions.Description.Infrastructure;

public class GoalDescriptionService
{
    private ILogger<GoalDescriptionService> _logger;
    private IDbContextFactory<AppContext> _contextFactory;

    public GoalDescriptionService(
        ILogger<GoalDescriptionService> logger,
        IDbContextFactory<AppContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<GoalDescription?> UpdateDescription(Goal goal, string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            description = null;
        }
        
        _logger.LogInformation("Updating goal description for goal {@Goal} to {Description}", goal, description);
        
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entry = await context.GoalDescriptions
            .SingleOrDefaultAsync(eg => eg.GoalId == goal.Id);

        if (description is not null)
        {
            entry ??= new()
            {
                GoalId = goal.Id,
                Description = description,
            };

            entry.Description = description;

            context.Update(entry);
        }
        else
        {
            if (entry is not null)
            {
                context.Remove(entry);
                entry = null;
            }
            else
            {
                return null;
            }
        }

        await context.SaveChangesAsync();

        return entry;
    }
}