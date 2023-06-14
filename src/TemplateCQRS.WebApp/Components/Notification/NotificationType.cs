using System.ComponentModel;

namespace TemplateCQRS.WebApp.Components.Notification;

public enum NotificationType
{
    [Description("Notify the user that the action has been completed without errors")]
    Success = 0,
    [Description("Notify the user that a message has been delivered")]
    Info = 1,
    [Description("Notify the user that something can go wrong")]
    Warning = 2,
    [Description("Notify the user that something causes the program to fail to meet a goal")]
    Error = 3
}