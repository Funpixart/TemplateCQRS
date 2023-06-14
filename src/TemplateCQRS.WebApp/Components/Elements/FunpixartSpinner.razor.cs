using TemplateCQRS.WebApp.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace TemplateCQRS.WebApp.Components.Elements;

public partial class FunpixartSpinner
{
    [Parameter] public string CustomCss { get; set; } = string.Empty;
    [Parameter] public SpinnerType SpinnerType { get; set; } = SpinnerType.Circle;
}