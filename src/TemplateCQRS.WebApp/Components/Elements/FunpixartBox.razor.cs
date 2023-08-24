using Microsoft.AspNetCore.Components;

namespace TemplateCQRS.WebApp.Components.Elements;

public partial class FunpixartBox : FunpixartComponent
{
    #region HEADER
    /// <summary>
    ///     Gets or sets the Title for the Header in this box
    /// </summary>
    [Parameter] public string HeaderTitle { get; set; } = "";
    /// <summary>
    ///     Gets or sets the styles for the Header in this box
    /// </summary>
    [Parameter] public string HeaderStyle { get; set; } = "";
    /// <summary>
    ///     Gets or sets the class for the Header in this box
    /// </summary>
    [Parameter] public string HeaderClass { get; set; } = "";
    /// <summary>
    ///     Gets or sets a ChildComponent in the Header if is defined
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; } = null!;

    #endregion
    
    #region CONTENT
    /// <summary>
    ///     Gets or sets a class for the Content of this box
    /// </summary>
    [Parameter] public string ContentClass { get; set; } = "";
    /// <summary>
    ///     Gets or sets styles for the Content of this box
    /// </summary>
    [Parameter] public string ContentStyle { get; set; } = "";
    /// <summary>
    ///     Gets or sets a ChildComponent for the Content if is defined
    /// </summary>
    [Parameter] public RenderFragment? Content { get; set; } = null!;

    #endregion

    #region FOOTER
    /// <summary>
    ///     Gets or sets styles for the Footer of this box
    /// </summary>
    [Parameter] public string FooterStyle { get; set; } = "";
    /// <summary>
    ///     Gets or sets a class for the Footer of this box
    /// </summary>
    [Parameter] public string FooterClass { get; set; } = "";
    /// <summary>
    ///     Gets or sets a ChildComponent in Footer if is defined
    /// </summary>
    [Parameter] public RenderFragment? Footer { get; set; } = null!;
    #endregion
}