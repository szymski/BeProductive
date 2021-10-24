using AntDesign;

namespace BeProductive.Modules.Common.Infrastructure;

public static class NotificationServiceExtensions
{
    public static void Success(this NotificationService service, string message) =>
        service.Success(new() { Message = message });
    
    public static void Error(this NotificationService service, string message) =>
        service.Error(new() { Message = message });
    
    public static void Warn(this NotificationService service, string message) =>
        service.Warn(new() { Message = message });
    
    public static void Info(this NotificationService service, string message) =>
        service.Info(new() { Message = message });
}