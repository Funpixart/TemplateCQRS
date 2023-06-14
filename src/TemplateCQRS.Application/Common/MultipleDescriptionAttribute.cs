using TemplateCQRS.Domain.Common;
using System.ComponentModel;

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