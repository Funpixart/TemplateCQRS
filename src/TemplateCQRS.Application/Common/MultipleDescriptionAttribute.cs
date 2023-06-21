using System.ComponentModel;
using TemplateCQRS.Domain.Common;

namespace TemplateCQRS.Application.Common;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class MultipleDescriptionAttribute : DescriptionAttribute
{
    public Language Language { get; set; }

    public MultipleDescriptionAttribute(string description, Language language)
        : base(description)
    {
        Language = language;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ResponseDescriptionAttribute : Attribute
{
    public int StatusCode { get; }
    public string Description { get; }

    public ResponseDescriptionAttribute(int statusCode, string description)
    {
        StatusCode = statusCode;
        Description = description;
    }
}