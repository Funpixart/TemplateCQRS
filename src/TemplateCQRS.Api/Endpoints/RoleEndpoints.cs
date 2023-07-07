using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemplateCQRS.Application.Attributes;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Queries;
using TemplateCQRS.Domain.Common;

namespace TemplateCQRS.Api.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        app.MapGet(ApiRoutes.Roles, GetAll)
            .CacheOutput(CachePolicy.GetRoles.Name);

        app.MapPost($"{ApiRoutes.Roles}{{role}}", CreateRole);

        app.MapPut($"{ApiRoutes.Roles}{{roleId}}", UpdateRole);

        app.MapDelete($"{ApiRoutes.Roles}{{roleId}}", DeleteRole);
    }

    [SwaggerSummary("Lista de roles")]
    [SwaggerDescription("Este endpoint retorna una lista de roles.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> GetAll(IMediator mediator)
    {
        var query = new GetAllRoleQuery();
        var result = await mediator.Send(query);

        return result.Match(
            success
                => success.Any() ? Results.Ok(success) : Results.NoContent(),
            failure
                => failure.Any(x => x.ErrorCode == StatusCodes.Status204NoContent.ToString()) 
                    ? Results.NoContent() : Results.BadRequest(failure));
    }

    [SwaggerSummary("Agrega un role")]
    [SwaggerDescription("Este endpoint agrega un role y retorna la nueva entidad agregada.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> CreateRole(CreateRoleCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Created(ApiRoutes.Roles, success),
            failure
                => failure.Any(x => x.ErrorCode == StatusCodes.Status204NoContent.ToString()) 
                    ? Results.NoContent() : Results.BadRequest(failure));
    }

    [SwaggerSummary("Actualiza un role")]
    [SwaggerDescription("Este endpoint actualiza un role y retorna la entidad actualizada.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> UpdateRole(UpdateRoleCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Accepted(ApiRoutes.Roles, success),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Elimina un role")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> DeleteRole(Guid id, IMediator mediator)
    {
        var command = new DeleteRoleCommand(id);
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Ok(success),
            failure
                => Results.BadRequest(failure));
    }
}