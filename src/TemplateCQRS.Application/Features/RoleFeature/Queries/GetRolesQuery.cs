using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.RoleFeature.Queries;

public record GetAllRoleQuery : IRequest<Payload<List<InfoRoleDto>, List<ValidationFailure>>>;