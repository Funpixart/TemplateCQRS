using Microsoft.AspNetCore.Components;

namespace TemplateCQRS.WebApp.Components.Elements;

public class ModalComponent : FunpixartComponent
{
    /// <summary>
    ///     Gets or sets the class CSS for the <see cref="ChildComponent"/>
    /// </summary>
    [Parameter] public string ContentClass { get; set; } = "";

    /// <summary>
    ///     Gets or sets the style for the <see cref="ChildComponent"/>
    /// </summary>
    [Parameter] public string ContentStyle { get; set; } = "";

    /// <summary>
    ///     Gets or sets the ChildComponent
    /// </summary>
    [Parameter] public RenderFragment? ChildComponent { get; set; }

    [Parameter] public EventCallback CallExit { get; set; }
}