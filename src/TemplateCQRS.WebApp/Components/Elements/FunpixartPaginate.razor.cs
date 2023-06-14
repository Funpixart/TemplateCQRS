using Microsoft.AspNetCore.Components;
using TemplateCQRS.WebApp.Data.Pagination;

namespace TemplateCQRS.WebApp.Components.Elements;

public partial class FunpixartPaginate : FunpixartComponent
{
    /// <summary>
    ///     Gets or sets the list of page links for the pagination component.
    /// </summary>
    [Parameter]
    public List<PagingLink> PagingLinks { get; set; } = new();

    /// <summary>
    ///     Event triggered when a page is selected in the pagination component.
    /// </summary>
    [Parameter]
    public EventCallback<PagingLink> OnSelectedPage { get; set; }
}