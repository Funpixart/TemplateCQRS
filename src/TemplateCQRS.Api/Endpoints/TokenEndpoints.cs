
using TemplateCQRS.Api.Security;
using TemplateCQRS.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace TemplateCQRS.Api.Endpoints;

public static class TokenEndpoints
{
    public static void MapTokenEndpoints(this WebApplication app)
    {
        app.MapPost("/security/requestToken", RequestToken).AllowAnonymous();
    }

    public static async Task<IResult> RequestToken(TokenGenerationRequest request, SignInManager<User> signInManager, IConfiguration config)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Results.NotFound("User not found.");
        }

        var attempt = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!attempt.Succeeded || attempt.IsNotAllowed)
        {
            return Results.Unauthorized();
        }

        // TODO: Add verification for roles and permissions 

        var tokenGenerationRequest = new TokenGenerationRequest
        {
            Email = user.Email ?? "",
            UserId = user.Id.ToString(),
        };

        var token = TokenHelper.GenerateToken(tokenGenerationRequest, config, 15);
        return Results.Ok(new { Token = token });
    }
}