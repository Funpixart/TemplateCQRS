using Serilog;
using System.Collections.ObjectModel;

namespace TemplateCQRS.WebApp.Components.Notification;

public interface INotificationService
{
    ObservableCollection<NotificationMessage> Messages { get; }
    void SendNotification(NotificationMessage message);
    void SendNotification(NotificationType severity);
    void SendNotification(NotificationType severity, string content);
    void SendNotificationForContext<T>(NotificationType severity, string content);
    void SendNotification(NotificationType severity, string content, int duration);
    void SendNotification(NotificationType severity, string content, string footer);
}

public class NotificationService : INotificationService
{
    public ObservableCollection<NotificationMessage> Messages { get; } = new();

    public void SendNotification(NotificationMessage message)
    {
        message ??= NotificationMessage.Default;

        if (Messages.Contains(message)) return;
        Messages.Add(message);
        LogNotification(message);
    }

    /// <summary>
    ///     Notify with just the notification type.
    /// </summary>
    /// <param name="severity">Notification type.</param>
    public void SendNotification(NotificationType severity)
    {
        SendNotification(new NotificationMessage { Severity = severity });
    }

    /// <summary>
    ///     Notify with notification type and a content.
    /// </summary>
    /// <param name="severity">Notification type.</param>
    /// <param name="content">Message of the notification.</param>
    public void SendNotification(NotificationType severity, string content)
    {
        SendNotification(new NotificationMessage { Message = content, Severity = severity });
    }

    /// <summary>
    ///     Notify with notification type and a content and the Type where is throwing the error.
    /// </summary>
    /// <param name="severity">Notification type.</param>
    /// <param name="content">Message of the notification.</param>
    public void SendNotificationForContext<T>(NotificationType severity, string content)
    {
        SendNotification(new NotificationMessage { Message = content, Severity = severity, Footer = $"{typeof(T).Name}" });
    }

    /// <summary>
    ///     Notify with notification type, content and more than 5 seconds.
    /// </summary>
    /// <param name="severity">Notification type.</param>
    /// <param name="content">Message of the notification.</param>
    /// <param name="duration">Duration in seconds of the notification before it disapears.</param>
    public void SendNotification(NotificationType severity, string content, int duration)
    {
        SendNotification(new NotificationMessage { Message = content, Severity = severity, Duration = duration * 1000 });
    }

    /// <summary>
    ///     Notify with notification type, content and footer.
    /// </summary>
    /// <param name="severity">Notification type.</param>
    /// <param name="content">Message of the notification.</param>
    /// <param name="footer">State or Code information about the notification prompt.</param>
    public void SendNotification(NotificationType severity, string content, string footer)
    {
        SendNotification(new NotificationMessage { Message = content, Severity = severity, Footer = $"{footer}" });
    }

    protected virtual void LogNotification(NotificationMessage message)
    {
        var footer = string.IsNullOrEmpty(message.Footer) ? "NotificationService" : message.Footer;
        switch (message.Severity)
        {
            case NotificationType.Error:
                Log.Logger.ForContext("Payload", footer).Error("{0}", message.Message);
                break;
            case NotificationType.Warning:
                Log.Logger.ForContext("Payload", footer).Warning("{0}", message.Message);
                break;
            case NotificationType.Info:
                Log.Logger.ForContext("Payload", footer).Information("{0}", message.Message);
                break;
            case NotificationType.Success:
                Log.Logger.ForContext("Payload", footer).Information("{0}", message.Message);
                break;
            default:
                Log.Logger.ForContext("Payload", footer).Warning("Notification failed to get Severity. But the message is: {0}", message.Message);
                break;
        }
    }
}