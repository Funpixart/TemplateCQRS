using Microsoft.AspNetCore.Components;
using TemplateCQRS.WebApp.Components.Enums;

namespace TemplateCQRS.WebApp.Components.Elements;

public partial class FunpixartSpinner
{
    [Parameter] public string CustomCss { get; set; } = string.Empty;
    [Parameter] public SpinnerType SpinnerType { get; set; } = SpinnerType.Circle;
}