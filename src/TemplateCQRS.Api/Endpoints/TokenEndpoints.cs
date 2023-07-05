using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using TemplateCQRS.Api.Security;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.UserFeature.Queries;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Dto.User;
using TemplateCQRS.Domain.Models;

namespace TemplateCQRS.Api.Endpoints;

public static class TokenEndpoints
{
    public static void MapTokenEndpoints(this WebApplication app)
    {
        app.MapPost(ApiRoutes.RequestToken, RequestToken)
            .AllowAnonymous();
    }

    [SwaggerSummary("Generar token")]
    [SwaggerDescription("Genera un token con un usuario y contrasena autorizado. Este token es valido segun el tiempo que se ha establecido en el desarrollo.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ResponseDescription(StatusCodes.Status400BadRequest, "Not Found, Unauthorized or Forbid")]
    public static async Task<IResult> RequestToken(UserTokenRequest request, SignInManager<User> signInManager,
        RoleManager<Role> roleManager, IConfiguration config, IMediator mediator)
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

        var getUserByQuery = new GetUserByQuery(new GetUserDto { Email = request.Email });
        var result = await mediator.Send(getUserByQuery);

        var tokenGenerationRequest = new TokenGenerationRequest
        {
            Email = user.Email ?? "",
            Password = request.Password,
            UserId = user.Id.ToString()
            // TODO add custom claims to the token.
        };

        var token = TokenHelper.GenerateToken(tokenGenerationRequest, config, 15);

        var userTokenDto = new UserTokenDto
        {
            Token = token,
            UserInfo = result.Data
        };
        return result.Match(
            success
                => result.IsSuccess ? Results.Ok(userTokenDto) : Results.NoContent(),
            failure => Results.BadRequest());
    }
}