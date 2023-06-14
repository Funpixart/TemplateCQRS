using Microsoft.AspNetCore.Components;
using Serilog;

namespace TemplateCQRS.WebApp.Components.Notification;

public partial class RDKNotification
{
    [Parameter] public NotificationMessage Message { get; set; } = null!;

    [Inject] private INotificationService Service { get; set; } = null!;

    public bool Visible { get; set; } = true;

    public void Close()
    {
        try
        {
            if (Service.Messages.Any())
            {
                Service.Messages.Remove(Message);
                Message.Dispose();
            }
            Task.Delay(100).ContinueWith(r => { Visible = false; });
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.ToString());
        }
    }

    protected override void OnInitialized()
    {
        if (Message == null)
        {
            Visible = false;
            return;
        }

        Task.Delay(Message.Duration ?? 5000).ContinueWith(r => InvokeAsync(Close));
    }
}