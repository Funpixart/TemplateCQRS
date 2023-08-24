using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

namespace TemplateCQRS.WebApp.Endpoints;

public static class AccountEndpoint
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        app.MapGet("/account/logout", Logout);
    }

    /// <summary>
    ///     SignOut user from Cookies
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static async Task<IResult> Logout(IHttpContextAccessor context, HttpResponse response)
    {
        try
        {
            if (context?.HttpContext?.User?.Identity is null || !context.HttpContext.User.Identity.IsAuthenticated)
                return Results.NoContent();

            await response.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.LocalRedirect("/");
        }
        catch (Exception ex)
        {
            Log.Logger.Error("{0}", ex);
            await Task.CompletedTask;
            return Results.LocalRedirect("/login");
        }
    }
}