using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.UserFeature.Commands;

public record CreateUserCommand(Guid? RoleId, CreateUserDto CreateUserDto) : IRequest<Payload<InfoUserDto, List<ValidationFailure>>>;

public record DeleteUserCommand(Guid Id) : IRequest<Payload<Unit, List<ValidationFailure>>>;

public record UpdateUserCommand(Guid Id, UpdateUserDto UpdateUserDto, Guid? RoleId) : IRequest<Payload<InfoUserDto, List<ValidationFailure>>>;
