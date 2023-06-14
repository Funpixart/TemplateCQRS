using Microsoft.AspNetCore.Components;

namespace TemplateCQRS.WebApp.Components.Elements;

public partial class FunpixartDropDown<TItem, TValue> : FunpixartDynamic
{
    [Parameter] public IEnumerable<TItem> Options { get; set; }
    [Parameter] public string TextProperty { get; set; }
    [Parameter] public string ValueProperty { get; set; }
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
    [Parameter] public TValue Value { get; set; }
    [Parameter] public string PlaceholderOption { get; set; } = "Select an option.";
    [Parameter] public bool IsPrimitiveType { get; set; }

    protected TValue CurrentValue
    {
        get => Value;
        set
        {
            if (EqualityComparer<TValue>.Default.Equals(Value, value)) return;
            Value = value;
            ValueChanged.InvokeAsync(Value);
        }
    }
}