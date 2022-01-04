using BeProductive.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Users.Infrastructure;

public class UserService
{
    private readonly IDbContextFactory<AppContext> _contextFactory;
    private ILogger<UserService> _logger;
    private AuthService _authService;
    private UserManager<User> _userManager;

    public UserService(
        ILogger<UserService> logger,
        AuthService authService,
        UserManager<User> userManager,
        IDbContextFactory<AppContext> contextFactory)
    {
        _logger = logger;
        _authService = authService;
        _userManager = userManager;
        _contextFactory = contextFactory;
    }
    
    public async Task<User?> GetUserById(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Users.FindAsync(id);
    }

    public async Task ChangeUsername(string username)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var authData = await _authService.GetAuthDataAsync();
        var user = await _userManager.FindByIdAsync(authData!.UserId.ToString());

        if (await _userManager.FindByNameAsync(username) is not null)
        {
            _logger.LogWarning("User {@User} tried to change nickname to already existing {NewUsername}", user, username);
            throw new("Username already exists");
        }

        _logger.LogInformation("Updating user {@User} username to {Username}", user, username);
        await _userManager.SetUserNameAsync(user, username);

        await context.SaveChangesAsync();

        await _authService.Login(user);
    }
    
    public async Task ChangePassword(string password)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var authData = await _authService.GetAuthDataAsync();
        var user = await context.Users.FindAsync(authData!.UserId);

        _logger.LogInformation("Updating user {@User} password", user);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _userManager.ResetPasswordAsync(user, token, password);

        await context.SaveChangesAsync();

        await _authService.Login(user);
    }
}