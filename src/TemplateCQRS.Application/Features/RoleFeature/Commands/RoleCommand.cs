using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.RoleFeature.Commands;

public record CreateRoleCommand(CreateRoleDto CreateRoleDto) : IRequest<Payload<InfoRoleDto, List<ValidationFailure>>>;

public record DeleteRoleCommand(Guid Id) : IRequest<Payload<Unit, List<ValidationFailure>>>;

public record UpdateRoleCommand(Guid Id, UpdateRoleDto UpdateRoleDto) : IRequest<Payload<InfoRoleDto, List<ValidationFailure>>>;