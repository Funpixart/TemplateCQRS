namespace TemplateCQRS.Application.Common;

[AttributeUsage(AttributeTargets.Method)]
public class SwaggerSummaryAttribute : Attribute
{
    public string Summary { get; set; }

    public SwaggerSummaryAttribute(string summary)
    {
        Summary = summary;
    }
}