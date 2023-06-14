namespace TemplateCQRS.WebApp.Components.Notification;

public class NotificationMessage : IDisposable
{
    public string? Title { get; set; }
    public string Message { get; set; } = "";
    public string Footer { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
    public int? Duration { get; set; }
    public NotificationType Severity { get; set; } = NotificationType.Info;
    public bool WasDisposed { get; set; } = false;

    public static readonly NotificationMessage Default = new()
    {
        Title = "Info",
        Message = "This is a default constructed message.",
        Footer = "default.",
        Date = DateTime.Now,
        Duration = 5000,
        Severity = NotificationType.Info
    };

    public string DateString =>
        $"{Date.Day,2:00}-{Date.Month,2:00}-{Date.Year,4:00} {Date.Hour,2:00}:{Date.Minute,2:00}";

    public string ImgSrc => $"img/{Severity}.png";

    public string ClassByType => $"{Severity.ToString().ToLower()}";

    public string TitleByType => Title ??= Severity + "!";

    public void Dispose()
    {
        WasDisposed = true;
        GC.SuppressFinalize(this);
    }
}