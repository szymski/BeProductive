using AntDesign;

namespace BeProductive.Modules.Common.Infrastructure;

public static class NotificationServiceExtensions
{
    public static void Success(this MessageService service, string message) =>
        service.Success(message);
    
    public static void Error(this MessageService service, string message) =>
        service.Error(message);
    
    public static void Warn(this MessageService service, string message) =>
        service.Warn(message);
    
    public static void Info(this MessageService service, string message) =>
        service.Info(message);
}