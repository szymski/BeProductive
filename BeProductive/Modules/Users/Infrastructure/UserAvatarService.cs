using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OneOf.Types;
using Storage.Net;
using Storage.Net.Blobs;

namespace BeProductive.Modules.Users.Infrastructure;

public class UserAvatarService {
    public class Options {
        public string Directory { get; set; }
    }

    private readonly Options _options;
    private readonly IDbContextFactory<AppContext> _contextFactory;
    private ILogger<UserAvatarService> _logger;
    private readonly IBlobStorage _storage;

    public UserAvatarService(
        IDbContextFactory<AppContext> contextFactory,
        ILogger<UserAvatarService> logger,
        IOptionsMonitor<Options> optionsMonitor)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _options = optionsMonitor.CurrentValue;
        _storage = StorageFactory.Blobs.DirectoryFiles(_options.Directory);
    }

    public async Task<string?> GetUserAvatarUrl(int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("Attempt to get avatar for non-existing user id {userId}", userId);
            return null;
        }
        
        return user.AvatarFile is null ? null : $"/api/Avatar";

    }
    
    public async Task<Stream?> GetUserAvatar(int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("Attempt to get avatar for non-existing user id {userId}", userId);
            return null;
        }

        if(user.AvatarFile is null)
        {
            return null;
        }
        
        return await GetUserAvatarByName(user.AvatarFile);
    }
    
    public async Task<Stream?> GetUserAvatarByName(string name)
    {
        return await _storage.OpenReadAsync(name);
    }
    
    public async Task<bool> UpdateAvatar(int userId, Stream newAvatar)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("Attempt to update avatar for non-existing user id {userId}", userId);
            return false;
        }

        _logger.LogInformation("Updating avatar for user id {@user}", user);

        if (user.AvatarFile is not null)
        {
            await _storage.DeleteAsync(user.AvatarFile);
            user.AvatarFile = null;
        }
        
        var filename = $"{Guid.NewGuid().ToString()}.png";
        await _storage.WriteAsync(filename, newAvatar);
        user.AvatarFile = filename;

        await context.SaveChangesAsync();

        return false;
    }
}