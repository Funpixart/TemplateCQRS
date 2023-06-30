using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Application.Features.ClaimFeature.Queries;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Dto.Claim;

namespace TemplateCQRS.Api.Endpoints;

public static class ClaimEndpoints
{
    public static void MapClaimEndpoints(this WebApplication app)
    {
        app.MapGet(ApiRoutes.Claim, GetAll);

        app.MapGet($"{ApiRoutes.Claim}{{roleId}}", GetAllById);

        app.MapPost($"{ApiRoutes.Claim}{{claim}}", CreateClaim);

        app.MapPut($"{ApiRoutes.Claim}{{claimId}}", UpdateClaim);

        app.MapDelete($"{ApiRoutes.Claim}{{claimId}}", DeleteClaim);
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
                => failure.Any(x => x.ErrorCode == StatusCodes.Status204NoContent.ToString()) 
                    ? Results.NoContent() : Results.BadRequest(failure));
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
                => failure.Any(x => x.ErrorCode == StatusCodes.Status204NoContent.ToString())
                    ? Results.NoContent() : Results.BadRequest(failure));
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
                => Results.Created(ApiRoutes.Claim, success),
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
                => Results.Accepted(ApiRoutes.Claim, success),
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

