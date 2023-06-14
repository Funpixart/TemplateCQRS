using TemplateCQRS.WebApp.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace TemplateCQRS.WebApp.Components.Elements;

public class BadgeComponent : FunpixartComponent
{
    /// <summary>
    ///     Gets or sets one of the badge style
    /// </summary>
    [Parameter] public BadgeStyle BadgeStyle { get; set; } = BadgeStyle.Default;
    /// <summary>
    ///     <c>If</c> true, render the Darker version
    /// </summary>
    [Parameter] public Shade Shade { get; set; } = Shade.Default;
    /// <summary>
    ///     If true, render the badge without the dot
    /// </summary>
    [Parameter] public bool Simple { get; set; }
    /// <summary>
    ///     Use this to defined the desired badge
    /// </summary>
    /// <value><b>See more in the style file on</b> <see cref="Badge"/></value>
    [Parameter] public string CustomClassCSS { get; set; } = "";
    /// <summary>
    ///     Use this to defined the desired message
    /// </summary>
    [Parameter] public string Text { get; set; } = "";

    protected string GetStatus()
    {
        if (!string.IsNullOrEmpty(CustomClassCSS)) return CustomClassCSS;

        var style = BadgeStyle.ToString().ToLowerInvariant();
        var shade = GetShade();

        return $"{style}{shade}";
    }

    protected string GetBadge() => !Simple ? "badge" : "badge-simple";

    protected string GetMessage() => string.IsNullOrEmpty(Text) ? BadgeStyle.ToString() : Text;

    private string GetShade() => Shade is not Shade.Default ? $"-{Shade.ToString().ToLowerInvariant()}" : "";
}