using FluentValidation.Results;
using TemplateCQRS.Domain.Dto.User;
using MediatR;

namespace TemplateCQRS.Application.Features.UserFeature.Commands;

public record CreateUserCommand(CreateUserDto CreateUserDto) : IRequest<Payload<DetailUserDto, List<ValidationFailure>>>;