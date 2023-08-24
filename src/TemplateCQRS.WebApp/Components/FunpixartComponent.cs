using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Serilog;
using TemplateCQRS.WebApp.Components.Enums;
using TemplateCQRS.WebApp.Components.Notification;
using TemplateCQRS.WebApp.Components.Services;

namespace TemplateCQRS.WebApp.Components;

/// <summary>
///     Base class of Funpixart Components
/// </summary>
public class FunpixartComponent : ComponentBase, IDisposable
{
    private DotNetObjectReference<FunpixartComponent> reference;

    internal bool Disposed = false;

    /// <summary>
    ///     Gets the reference for the current component.
    /// </summary>
    /// <value>The reference.</value>
    protected DotNetObjectReference<FunpixartComponent> Reference => reference ??= DotNetObjectReference.Create(this);

    /// <summary>
    ///     Gets a reference to the HTML element rendered by the component.
    /// </summary>
    public ElementReference Element { get; internal set; }

    /// <summary>
    ///     Gets or sets the unique identifier.
    /// </summary>
    /// <value>The unique identifier.</value>
    protected string UniqueId { get; set; } = string.Empty;

    /// <summary>
    ///     Inject the <see cref="INotificationService"/>.
    /// </summary>
    [Inject]
    protected INotificationService NotifyService { get; set; } = null!;

    /// <summary>
    ///     Inject the <see cref="Services.FunpixartServiceJs"/>.
    /// </summary>
    /// <value>The <see cref="Services.IFunpixartServiceJs"/></value>
    [Inject]
    protected IFunpixartServiceJs JsService { get; set; } = null!;

    /// <summary>
    ///     Inject the <see cref="NavigationManager"/> services
    /// </summary>
    [Inject]
    protected NavigationManager UriHelper { get; set; } = null!;

    /// <summary>
    ///     Specifies additional custom attributes that will be rendered by the component.
    /// </summary>
    /// <value>The attributes.</value>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; } = new();

    /// <summary>
    ///     Gets or sets a value indicating whether this <see cref="FunpixartComponent"/> is visible. Invisible components are not rendered.
    /// </summary>
    /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
    [Parameter]
    public virtual bool Visible { get; set; } = true;

    /// <summary>
    ///     <c>If</c> true, render the Darker version
    /// </summary>
    [Parameter] public Shade Shade { get; set; } = Shade.Default;
    
    /// <summary>
    ///     Gets or sets one of the badge style
    /// </summary>
    [Parameter] public Style Style { get; set; } = Style.Default;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        UniqueId = await GenerateElementId();
    }

    private static Task<string> GenerateElementId()
    {
        var base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return Task.FromResult(base64Guid[..10].Replace('+', '-').Replace('/', '-'));
    }

    /// <summary>
    ///     Called by the Blazor runtime when parameters are set.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
    }

    /// <summary>
    ///     Gets the unique identifier. 
    /// </summary>
    /// <returns>Returns the <c>id</c> attribute (if specified) or generates a random one.</returns>
    protected string GetId()
    {
        return Attributes.Count > 0 && Attributes.TryGetValue("id", out var id) && !string.IsNullOrEmpty(Convert.ToString(@id))
            ? $"{@id}"
            : UniqueId;
    }

    protected string GetShade() => Shade is not Shade.Default ? $"-{Shade.ToString().ToLowerInvariant()}" : "";

    protected string GetStyle() => Style.ToString().ToLowerInvariant();

    public virtual void Dispose()
    {
        Disposed = true;
        reference?.Dispose();
        reference = null!;

        GC.SuppressFinalize(this);
    }
}