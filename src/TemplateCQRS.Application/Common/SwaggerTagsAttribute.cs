namespace TemplateCQRS.Application.Common;

[AttributeUsage(AttributeTargets.Method)]
public class SwaggerTagsAttribute : Attribute
{
    public string[] Tags { get; set; }

    public SwaggerTagsAttribute(params string[] tags)
    {
        Tags = tags;
    }
}