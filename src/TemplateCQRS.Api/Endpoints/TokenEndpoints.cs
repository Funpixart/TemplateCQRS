using TemplateCQRS.Api.Security;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using TemplateCQRS.Api.Security;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Models;
using static TemplateCQRS.Domain.Common.Constants;

namespace TemplateCQRS.Api.Endpoints;

public static class TokenEndpoints
{
    public static void MapTokenEndpoints(this WebApplication app)
    {
        app.MapPost(ApiRoutes.SecurityRoutes.RequestToken, RequestToken)
            .AllowAnonymous();
    }

    [SwaggerSummary("Generar token")]
    [SwaggerDescription("Genera un token con un usuario y contrasena autorizado. Este token es valido segun el tiempo que se ha establecido en el desarrollo.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ResponseDescription(StatusCodes.Status400BadRequest, "Not Found, Unauthorized or Forbid")]
    public static async Task<IResult> RequestToken(UserTokenRequest request, SignInManager<User> signInManager, IConfiguration config)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        var attempt = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!attempt.Succeeded || attempt.IsNotAllowed)
        {
            return Results.Unauthorized();
        }

        // TODO: Add verification for roles and permissions 

        var userClaims = await signInManager.UserManager.GetClaimsAsync(user);
        var tokenGenerationRequest = new TokenGenerationRequest
        {
            Email = user.Email ?? "",
            Password = request.Password,
            UserId = user.Id.ToString(),
            CustomClaims = userClaims.ToDictionary(claim => claim.Type, claim => JsonDocument.Parse(claim.Value).RootElement)
        };

        var token = TokenHelper.GenerateToken(tokenGenerationRequest, config, 15);
        return Results.Ok(new { Token = token });
    }
}