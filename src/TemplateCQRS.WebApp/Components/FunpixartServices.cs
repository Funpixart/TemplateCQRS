using TemplateCQRS.WebApp.Components.Notification;
using TemplateCQRS.WebApp.Components.Services;

namespace TemplateCQRS.WebApp.Components;

public static class FunpixartServices
{
    public static void AddFunpixartServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IFunpixartServiceJs, FunpixartServiceJs>();
    }
}