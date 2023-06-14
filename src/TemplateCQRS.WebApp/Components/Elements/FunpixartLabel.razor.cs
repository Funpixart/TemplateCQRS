using Microsoft.AspNetCore.Components;

namespace TemplateCQRS.WebApp.Components.Elements;

public partial class FunpixartLabel : FunpixartComponent
{
    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    /// <value>The child content.</value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Binds this label to an element.
    /// </summary>
    /// <value>The component name for the label.</value>
    [Parameter]
    public string For { get; set; } = "";

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    [Parameter]
    public string Text { get; set; } = "";
}