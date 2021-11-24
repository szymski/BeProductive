using BeProductive.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;

namespace BeProductive.Modules.Users.Infrastructure;

public class UserService
{
    private ILogger<UserService> _logger;
    private AppContext _context;
    private AuthService _authService;
    private UserManager<User> _userManager;

    public UserService(
        ILogger<UserService> logger,
        AppContext context,
        AuthService authService,
        UserManager<User> userManager)
    {
        _logger = logger;
        _context = context;
        _authService = authService;
        _userManager = userManager;
    }

    public async Task ChangeUsername(string username)
    {
        var authData = await _authService.GetAuthDataAsync();
        var user = await _context.Users.FindAsync(authData!.UserId);

        if (await _userManager.FindByNameAsync(username) is not null)
        {
            _logger.LogWarning("User {@User} tried to change nickname to already existing {NewUsername}", user, username);
            throw new("Username already exists");
        }

        _logger.LogInformation("Updating user {@User} username to {Username}", user, username);
        await _userManager.SetUserNameAsync(user, username);

        await _context.SaveChangesAsync();

        await _authService.Login(user);
    }
}