using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using TemplateCQRS.Api.Security;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Application.Features.UserFeature.Queries;
using TemplateCQRS.Domain.Dto.User;
using TemplateCQRS.Domain.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TemplateCQRS.Api.Common;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Application.Features.UserFeature.Queries;
using TemplateCQRS.Domain.Dto.User;
using TemplateCQRS.Domain.Models;
using static TemplateCQRS.Domain.Common.Constants;

namespace TemplateCQRS.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet(ApiRoutes.UserRoutes.Users, GetAll);
            //.RequireAuthorization();

        app.MapPost($"{ApiRoutes.UserRoutes.Users}/{{user}}", CreateUser);
            //.RequireAuthorization();

        app.MapPut($"{ApiRoutes.UserRoutes.Users}/{{userId}}", UpdateUser);
            //.RequireAuthorization();

        app.MapDelete($"{ApiRoutes.UserRoutes.Users}/{{userId}}", DeleteUser);
            //.RequireAuthorization();

        app.MapPut(ApiRoutes.UserRoutes.UsersChangePassword, ChangePassword);
            //.RequireAuthorization();
    }

    [SwaggerSummary("Lista de usuarios")]
    [SwaggerDescription("Este endpoint retorna una lista de usuarios.")]
    [ProducesResponseType(typeof(List<InfoUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> GetAll(IMediator mediator)
    {
        var query = new GetAllUserQuery();
        var result = await mediator.Send(query);

        return result.Match(
            success
                => success.Any() ? Results.Ok(success) : Results.NoContent(),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Agrega un usuario")]
    [SwaggerDescription("Este endpoint agrega un usuario y retorna la nueva entidad agregada.")]
    [ProducesResponseType(typeof(InfoUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> CreateUser(CreateUserCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Created(ApiRoutes.UserRoutes.Users, success),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Actualiza un usuario")]
    [SwaggerDescription("Este endpoint actualiza un usuario y retorna la entidad actualizada.")]
    [ProducesResponseType(typeof(UpdateUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> UpdateUser(UpdateUserCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Accepted(ApiRoutes.UserRoutes.Users, success),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Elimina un usuario")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> DeleteUser(Guid id, IMediator mediator)
    {
        var command = new DeleteUserCommand(id);
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Ok(success),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Modificar la contraseña de un usuario")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> ChangePassword(UpdateUserPasswordDto userDto, SignInManager<User> signInManager)
    {
        var user = await signInManager.UserManager.FindByIdAsync(userDto.Id.ToString());
        if (user == null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        var attempt = await signInManager.CheckPasswordSignInAsync(user, userDto.CurrentPassword, false);
        if (!attempt.Succeeded || attempt.IsNotAllowed)
        {
            return Results.Unauthorized();
        }

        var token = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
        var result = await signInManager.UserManager.ResetPasswordAsync(user, token, userDto.NewPassword);

        return result.Succeeded
            ? Results.Ok(new { Message = "La contraseña ha sido actualizada!" })
            : Results.BadRequest(new { result.Errors });
    }
}