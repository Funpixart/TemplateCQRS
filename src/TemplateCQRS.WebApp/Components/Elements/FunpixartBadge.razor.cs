using Microsoft.AspNetCore.Components;
using TemplateCQRS.WebApp.Components.Enums;

namespace TemplateCQRS.WebApp.Components.Elements;

public class BadgeComponent : FunpixartComponent
{
    /// <summary>
    ///     If true, render the badge without the dot
    /// </summary>
    [Parameter] public bool Simple { get; set; }
    /// <summary>
    ///     Use this to defined the desired message
    /// </summary>
    [Parameter] public string Text { get; set; } = "";

    protected string GetStyle()
    {
        var style = SetStyle();
        var shade = SetShade();

        return $"{style}{shade}";
    }

    protected string GetBadge() => !Simple ? "badge" : "badge-simple";

    protected string GetMessage() => string.IsNullOrEmpty(Text) ? Style.ToString() : Text;
}