using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Application.Features.ClaimFeature.Queries;
using TemplateCQRS.Domain.Dto.Claim;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemplateCQRS.Domain.Common;

namespace TemplateCQRS.Api.Endpoints;

public static class ClaimEndpoints
{
    public static void MapClaimEndpoints(this WebApplication app)
    {
        app.MapGet(ApiRoutes.ClaimRoutes.Claim, GetAll);
        //  .RequireAuthorization();

        app.MapGet($"{ApiRoutes.ClaimRoutes.Claim}/{{roleId}}", GetAllById);
        // .RequireAuthorization();

        app.MapPost($"{ApiRoutes.ClaimRoutes.Claim}/{{claim}}", CreateClaim);
        //  .RequireAuthorization();

        app.MapPut($"{ApiRoutes.ClaimRoutes.Claim}/{{claimId}}", UpdateClaim);
        // .RequireAuthorization();

        app.MapDelete($"{ApiRoutes.ClaimRoutes.Claim}/{{claimId}}", DeleteClaim);
           // .RequireAuthorization();
    }

    [SwaggerSummary("Lista de permisos")]
    [SwaggerDescription("Este endpoint retorna una lista de permisos.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> GetAll(IMediator mediator)
    {
        var query = new GetAllClaimsQuery();
        var result = await mediator.Send(query);

        return result.Match(
            success
                => success.Any() ? Results.Ok(success) : Results.NoContent(),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Lista de permisos por role")]
    [SwaggerDescription("Este endpoint retorna una lista de permisos por role.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> GetAllById(Guid roleId, IMediator mediator)
    {
        var query = new GetAllClaimsByRoleQuery(roleId);
        var result = await mediator.Send(query);

        return result.Match(
            success
                => success.Any() ? Results.Ok(success) : Results.NoContent(),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Agrega un permiso")]
    [SwaggerDescription("Este endpoint agrega un permiso y retorna la nueva entidad agregada.")]
    [ProducesResponseType(typeof(CreateClaimDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> CreateClaim(CreateClaimCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Created(ApiRoutes.ClaimRoutes.Claim, success),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Actualiza un permiso")]
    [SwaggerDescription("Este endpoint actualiza un permiso y retorna la entidad actualizada.")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> UpdateClaim(UpdateClaimCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Accepted(ApiRoutes.ClaimRoutes.Claim, success),
            failure
                => Results.BadRequest(failure));
    }

    [SwaggerSummary("Elimina un permiso")]
    [ResponseDescription(StatusCodes.Status200OK, "Success")]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public static async Task<IResult> DeleteClaim(int id, IMediator mediator)
    {
        var command = new DeleteClaimCommand(id);
        var result = await mediator.Send(command);

        return result.Match(
            success
                => Results.Ok(success),
            failure
                => Results.BadRequest(failure));
    }
}
