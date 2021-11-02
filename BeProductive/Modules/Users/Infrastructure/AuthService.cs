using System.Security.Claims;
using BeProductive.Modules.Users.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace BeProductive.Modules.Users.Infrastructure;

public class AuthService
{
    private IJSRuntime _jsRuntime;
    private SignInManager<User> _signInManager;
    private IOptionsMonitor<CookieAuthenticationOptions> _cookieOptionsMonitor;
    private IHostEnvironmentAuthenticationStateProvider _hostAuthentication;
    private AuthenticationStateProvider _authenticationStateProvider;
    private ILogger<AuthService> _logger;

    public AuthService(
        IOptionsMonitor<CookieAuthenticationOptions> cookieOptionsMonitor,
        IHostEnvironmentAuthenticationStateProvider hostAuthentication,
        SignInManager<User> signInManager,
        IJSRuntime jsRuntime,
        ILogger<AuthService> logger, AuthenticationStateProvider authenticationStateProvider)
    {
        _cookieOptionsMonitor = cookieOptionsMonitor;
        _hostAuthentication = hostAuthentication;
        _signInManager = signInManager;
        _jsRuntime = jsRuntime;
        _logger = logger;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<bool> SignIn(string username, string password)
    {
        _logger.LogInformation("Attempting to sign in using username {Username} and password {Password}", username,
            password);

        var user = await _signInManager.UserManager.FindByNameAsync(username);
        if (user is null)
        {
            _logger.LogWarning("No such user with username {Username}", username);
            return false;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed login attempt for user {@User} using password {Password}", user, password);
            return false;
        }

        await Login(user);

        return true;
    }

    public async Task Login(User user)
    {
        _logger.LogInformation("Logging in user {@User}", user);

        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        var identity = new ClaimsIdentity(principal.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        principal = new(identity);
        _signInManager.Context.User = principal;
        _hostAuthentication.SetAuthenticationState(Task.FromResult(new AuthenticationState(principal)));

        var cookieOptions = _cookieOptionsMonitor
            .Get(CookieAuthenticationDefaults.AuthenticationScheme);

        var ticket = new AuthenticationTicket(principal, null, CookieAuthenticationDefaults.AuthenticationScheme);
        var value = cookieOptions.TicketDataFormat.Protect(ticket);
        _logger.LogDebug("Setting session cookie {Cookie} to {Value}", cookieOptions.Cookie.Name, value);
        await _jsRuntime.InvokeVoidAsync(
            "blazorExtensions.WriteCookie",
            cookieOptions.Cookie.Name,
            value,
            cookieOptions.ExpireTimeSpan.TotalDays
        );
    }

    public async Task Logout()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _logger.LogInformation("Logging out user {User}", state?.User?.Identity?.Name);

        var cookieOptions = _cookieOptionsMonitor
            .Get(CookieAuthenticationDefaults.AuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        _signInManager.Context.User = claimsPrincipal;
        _hostAuthentication.SetAuthenticationState(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        await _jsRuntime.InvokeVoidAsync("blazorExtensions.DeleteCookie", cookieOptions.Cookie.Name);
    }
}