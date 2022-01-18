using Append.Blazor.Notifications;

namespace BeProductive.Modules.Common.Infrastructure;

public class HtmlNotificationService {
    private readonly INotificationService _notificationService;

    public HtmlNotificationService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task RequestPermissions()
    {
        await _notificationService.RequestPermissionAsync();
    }
    
    public async Task Show(string title, string description, string icon = null)
    {
        var permission = _notificationService.PermissionStatus;
        if (permission == PermissionType.Default)
        {
            await _notificationService.RequestPermissionAsync();
        }
        await _notificationService.CreateAsync(title, description, icon);
    }
}