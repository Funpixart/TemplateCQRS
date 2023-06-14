using Serilog;
using System.Net;
using System.Text.Json;

namespace TemplateCQRS.WebApp.Data.Middleware;

public static class ExceptionsMiddleware
{
    /// <summary>
    ///     Extension method to add the ExceptionCatcherMiddleware to the IApplicationBuilder's middleware pipeline.
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance for which to add the ExceptionCatcherMiddleware.</param>
    /// <returns>The updated IApplicationBuilder instance with the ExceptionCatcherMiddleware added to the pipeline.</returns>
    public static IApplicationBuilder UseExceptionCatcherMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error occurred");
                var error = ex.InnerException is not null ? ex.InnerException.Message : ex.Message;
                await HandleExceptionAsync(context, new { unexpectedError = error }, (int)HttpStatusCode.InternalServerError);
            }
        });

        return app;
    }

    /// <summary>
    ///     Handles exceptions by setting the HTTP response with appropriate status code
    ///     and writes the error details into the response body asynchronously.
    /// </summary>
    /// <param name="context">The HttpContext which encapsulates all HTTP-specific information about an individual HTTP request.</param>
    /// <param name="errors">The object that contains the error details to be written into the response.</param>
    /// <param name="statusCode">The HTTP status code to be set in the response.</param>
    private static async Task HandleExceptionAsync(HttpContext context, object errors, int statusCode)
    {
        var jsonResponse = JsonSerializer.Serialize(errors);

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(jsonResponse);
    }
}