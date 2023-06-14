using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TemplateCQRS.WebApp.Components.Elements;

public partial class FunpixartButton : FunpixartDynamic
{
    /// <summary>
    ///     Gets or sets the OnClick event callback,
    ///     which is triggered when the button is clicked.
    /// </summary>
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    ///     Gets or sets the CSS class for the button's icon.
    /// </summary>
    [Parameter] public string IconClass { get; set; } = "";

    /// <summary>
    ///     Gets or sets a boolean value indicating
    ///     whether the button has an icon.
    /// </summary>
    [Parameter] public bool HasIcon { get; set; }

    /// <summary>
    ///     Gets or sets a boolean value indicating
    ///     whether the button has a text.
    /// </summary>
    [Parameter] public bool HasText { get; set; } = true;

    /// <summary>
    ///     Gets or sets the text displayed on the button.
    /// </summary>
    [Parameter] public string Text { get; set; } = "Button";

    /// <summary>
    ///     Gets or sets the CSS class for the button.
    /// </summary>
    [Parameter] public string ButtonClass { get; set; } = "";

    [Parameter] public string Type { get; set; } = "";

    [Parameter] public string Form { get; set; } = "";

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!IsDisabled && !string.IsNullOrEmpty(ButtonClass)) Attributes.TryAdd("class", ButtonClass);
        if (!string.IsNullOrEmpty(Type)) Attributes.TryAdd("type", Type);
        if (!string.IsNullOrEmpty(Form)) Attributes.TryAdd("form", Form);
    }
}