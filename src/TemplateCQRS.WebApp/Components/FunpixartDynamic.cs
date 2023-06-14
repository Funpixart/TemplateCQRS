using Microsoft.AspNetCore.Components;

namespace TemplateCQRS.WebApp.Components;

public class FunpixartDynamic : FunpixartComponent
{
    /// <summary>
    ///     Gets or sets a boolean value indicating
    ///     whether the element field is required.
    /// </summary>
    [Parameter] public bool IsRequired { get; set; }

    /// <summary>
    ///     Gets or sets a boolean value indicating
    ///     whether the element field is disabled.
    /// </summary>
    [Parameter] public bool IsDisabled { get; set; }

    /// <summary>
    ///     Gets or sets the pattern attribute for the element field, 
    ///     which specifies a regular expression the field's value should match.
    /// </summary>
    [Parameter] public string Pattern { get; set; } = "";

    /// <summary>
    ///     Gets or sets the title attribute for the element field, 
    ///     which provides additional information about the field.
    /// </summary>
    [Parameter] public string Title { get; set; } = "";

    /// <summary>
    ///     Gets or sets the placeholder attribute for the element field,
    ///     which provides a hint to the user of what can be entered in the field.
    /// </summary>
    [Parameter] public string Placeholder { get; set; } = "";

    [Parameter] public string Label { get; set; } = "";

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (IsDisabled) Attributes.TryAdd("disabled", true);
        if (IsRequired) Attributes.TryAdd("required", true);
        if (!string.IsNullOrEmpty(Title)) Attributes.TryAdd("title", Title);
        if (!string.IsNullOrEmpty(Pattern)) Attributes.TryAdd("pattern", Pattern);
        if (!string.IsNullOrEmpty(Placeholder)) Attributes.TryAdd("placeholder", Placeholder);
    }
}

public class FunpixartDynamic<TValue> : FunpixartDynamic
{
    /// <summary>
    ///     Gets or sets the value of the TValue object.
    /// </summary>
    [Parameter] public TValue? Value { get; set; }

    /// <summary>
    ///     Gets or sets the event callback that is
    ///     triggered when the TValue value changes.
    /// </summary>
    [Parameter] public EventCallback<TValue> CallbackValue { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Value = Value is null ? default : Value;
    }

    protected async void InvokeValue(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Value = (TValue)Convert.ChangeType(e.Value, typeof(TValue));

        await CallbackValue.InvokeAsync(Value);
    }
}