namespace TemplateCQRS.Application.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class SwaggerDescriptionAttribute : Attribute
{
    public string Description { get; set; }

    public SwaggerDescriptionAttribute(string description)
    {
        Description = description;
    }
}