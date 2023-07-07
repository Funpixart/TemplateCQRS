using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.AspNetCore.OutputCaching;
using TemplateCQRS.Api.Security;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.UserFeature.Queries;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Dto.User;
using TemplateCQRS.Domain.Models;
using TemplateCQRS.Application.Attributes;

namespace TemplateCQRS.Api.Endpoints;

public static class TokenEndpoints
{
    public static void MapTokenEndpoints(this WebApplication app)
    {
        app.MapPost(ApiRoutes.RequestToken, RequestToken)
            .AllowAnonymous().CacheOutput(CachePolicy.RequestToken.Name);
    }

    [SwaggerSummary("Generar token")]
    [SwaggerDescription("Genera un token con un usuario y contrasena autorizado. Este token es valido segun el tiempo que se ha establecido en el desarrollo.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ResponseDescription(StatusCodes.Status400BadRequest, "Not Found, Unauthorized or Forbid")]
    public static async Task<IResult> RequestToken(UserTokenRequest request,
        SignInManager<User> signInManager, UserManager<User> userManager,
        RoleManager<Role> roleManager, IConfiguration config, IMediator mediator, IOutputCacheStore cache)
    {
        if (string.IsNullOrEmpty(request.Email)) return Results.BadRequest(new { Message = "Email not provided." });
        if (string.IsNullOrEmpty(request.Password)) return Results.BadRequest(new { Message = "Password not provided." });

        // Find user if exist
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        // Check if the credentials are valid
        var attempt = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!attempt.Succeeded || attempt.IsNotAllowed)
        {
            return Results.Unauthorized();
        }

        // Try to retrieve the token from cache
        var userTokenDto = new UserTokenDto();
        var ct = new CancellationToken();

        var cachedToken = await cache.GetAsync(request.Email, ct);
        if (cachedToken is not null)
        {
            userTokenDto = JsonSerializer.Deserialize<UserTokenDto>(cachedToken);
            if (userTokenDto != null)
            {
                return Results.Ok(userTokenDto);
            }
        }

        var getUserByQuery = new GetUserByQuery(new GetUserDto { Email = request.Email });
        var result = await mediator.Send(getUserByQuery);

        var tokenGenerationRequest = new TokenGenerationRequest
        {
            Email = user.Email ?? "",
            Password = request.Password,
            UserId = user.Id.ToString()
            // TODO add custom claims to the token.
        };

        // Generate the token
        var token = TokenHelper.GenerateToken(tokenGenerationRequest, config, 30);

        userTokenDto = new UserTokenDto
        {
            Token = token,
            UserInfo = result.Data
        };

        // Cache the token with the info
        var userTokenSerialize = JsonSerializer.SerializeToUtf8Bytes(userTokenDto);
        await cache.SetAsync(request.Email, userTokenSerialize, new[] { CachePolicy.RequestToken.Tag },
            CachePolicy.RequestToken.Expiration, ct);

        return result.Match(
            success
                => result.IsSuccess ? Results.Ok(userTokenDto) : Results.NoContent(),
            failure => Results.BadRequest());
    }
}