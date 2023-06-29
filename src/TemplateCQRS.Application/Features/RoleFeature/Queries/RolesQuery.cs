using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.RoleFeature.Queries;

public record GetAllRoleQuery : IRequest<Payload<List<InfoRoleDto>, List<ValidationFailure>>>;

public record GetRoleByQuery(Guid? Id, string? Name) : IRequest<Payload<InfoRoleDto, List<ValidationFailure>>>;