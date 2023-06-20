using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace TemplateCQRS.Application.Common;

public class CustomOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodInfo = context.MethodInfo;
        var endpointDescriptionAttribute = methodInfo.GetCustomAttribute<SwaggerDescriptionAttribute>();
        var endpointTagsAttribute = methodInfo.GetCustomAttribute<SwaggerTagsAttribute>();
        var endpointSummaryAttribute = methodInfo.GetCustomAttribute<SwaggerSummaryAttribute>();

        if (endpointDescriptionAttribute is not null)
        {
            operation.Description = endpointDescriptionAttribute.Description;
        }

        if (endpointTagsAttribute is not null)
        {
            operation.Tags = endpointTagsAttribute.Tags.Select(tag => new OpenApiTag
            {
                Name = tag
            }).ToList();
        }

        if (endpointSummaryAttribute is not null)
        {
            operation.Summary = endpointSummaryAttribute.Summary;
        }

        // Handle response descriptions
        var responseDescriptionAttributes = methodInfo.GetCustomAttributes<ResponseDescriptionAttribute>();
        foreach (var attr in responseDescriptionAttributes)
        {
            if (!operation.Responses.ContainsKey(attr.StatusCode.ToString()))
            {
                operation.Responses.Add(attr.StatusCode.ToString(), new OpenApiResponse { Description = attr.Description });
            }
            else
            {
                operation.Responses[attr.StatusCode.ToString()].Description = attr.Description;
            }
        }
    }
}