using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace TemplateCQRS.WebApp.Data.Extensions;

public static class SerilogExtensions
{
    public static string Template { get; set; } = "{@t:dd/MM/yyyy HH-mm-ss} [{@l:u4}]: {#if SourceContext is not null}[{Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1) }] {#end}" +
                                                  "{#if Payload is not null}[{Payload}] {#end}{@m} \n" + "{@x}";

    public static Logger InitializeSerilog(IConfiguration config)
    {
        var logger = Config(config).CreateLogger();
        Log.Logger = logger;
        return logger;
    }

    /// <summary>
    ///     Custom configuration for serilog to show on console and save to file.
    /// </summary>
    public static LoggerConfiguration Config(IConfiguration config)
    {
        var date = $"{DateTime.Today.Day}_{DateTime.Today.Month}_{DateTime.Today.Year}";
        var logPath = Path.Combine(Environment.CurrentDirectory + "/",
            $"Logs/{AppDomain.CurrentDomain.FriendlyName}_{date}.json");
        JsonFormatter formatter = new(renderMessage: true);

        if (!File.Exists(logPath))
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory("Logs");
        return new LoggerConfiguration()
            .MinimumLevel.Override("Default", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .MinimumLevel.Verbose()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", AppDomain.CurrentDomain.FriendlyName)
            .Enrich.FromLogContext()
            .WriteTo.Console(new ExpressionTemplate(Template, theme: BaseTheme))
            .WriteTo.File(formatter,
                logPath,
                LogEventLevel.Warning,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 10000000);
    }

    public static TemplateTheme BaseTheme { get; } = new(
        new Dictionary<TemplateThemeStyle, string>
        {
            [TemplateThemeStyle.Text] = "\x1b[38;5;0240m",
            [TemplateThemeStyle.SecondaryText] = "\x1b[38;5;244m",
            [TemplateThemeStyle.TertiaryText] = "\x1b[38;5;0242m",
            [TemplateThemeStyle.Invalid] = "\x1b[0;31m",
            [TemplateThemeStyle.Null] = "\x1b[1;34m ",
            [TemplateThemeStyle.Name] = "\x1b[38;5;0081m",
            [TemplateThemeStyle.String] = "\x1b[0;36m",
            [TemplateThemeStyle.Number] = "\x1b[1;32m",
            [TemplateThemeStyle.Boolean] = "\x1b[1;34m ",
            [TemplateThemeStyle.Scalar] = "\x1b[38;5;0079m",
            [TemplateThemeStyle.LevelVerbose] = "\x1b[1;30m",
            [TemplateThemeStyle.LevelDebug] = "\x1b[38;5;7m",
            [TemplateThemeStyle.LevelInformation] = "\x1b[1;36m",
            [TemplateThemeStyle.LevelWarning] = "\x1b[0;33m",
            [TemplateThemeStyle.LevelError] = "\x1b[38;5;202m",
            [TemplateThemeStyle.LevelFatal] = "\x1b[38;5;166m",
        });
}