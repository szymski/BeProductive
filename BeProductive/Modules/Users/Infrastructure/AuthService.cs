using System.Security.Claims;
using System.Security.Principal;
using BeProductive.Modules.Users.Domain;
using BeProductive.Modules.Users.Infrastructure.Data;
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

    private AuthData? _currentAuthData;

    public AuthService(
        IOptionsMonitor<CookieAuthenticationOptions> cookieOptionsMonitor,
        IHostEnvironmentAuthenticationStateProvider hostAuthentication,
        SignInManager<User> signInManager,
        IJSRuntime jsRuntime,
        ILogger<AuthService> logger,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _cookieOptionsMonitor = cookieOptionsMonitor;
        _hostAuthentication = hostAuthentication;
        _signInManager = signInManager;
        _jsRuntime = jsRuntime;
        _logger = logger;
        _authenticationStateProvider = authenticationStateProvider;

        authenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateProviderOnAuthenticationStateChanged;

        OnAuthenticationStateProviderOnAuthenticationStateChanged(authenticationStateProvider.GetAuthenticationStateAsync());
    }
    
    private async void OnAuthenticationStateProviderOnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var state = await task;
        if (state.User.Identity?.IsAuthenticated != true)
        {
            _currentAuthData = null;
            return;
        }

        var userId = int.Parse(state.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var username = state.User.FindFirst(ClaimTypes.Name)!.Value;

        _currentAuthData = new(userId, username);
    }

    public async Task<bool> IsLoggedIn()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<IIdentity?> GetAuthState()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User.Identity?.IsAuthenticated == true ? state.User.Identity : null;
    }

    public async Task<IEnumerable<Claim>?> GetAuthClaims()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User.Identity?.IsAuthenticated == true ? state.User.Claims : null;
    }

    public async Task<AuthData?> GetAuthDataAsync()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (state.User.Identity?.IsAuthenticated != true) return null;

        var userId = int.Parse(state.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var username = state.User.FindFirst(ClaimTypes.Name)!.Value;

        return new(userId, username);
    }

    public AuthData? GetAuthData()
    {
        return _currentAuthData;
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

        _currentAuthData = new AuthData(user.Id, user.UserName);
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

        _currentAuthData = null;
    }
}