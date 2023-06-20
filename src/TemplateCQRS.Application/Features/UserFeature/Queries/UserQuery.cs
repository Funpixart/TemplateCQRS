using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.UserFeature.Queries;

public record GetAllUserQuery : IRequest<Payload<List<InfoUserDto>, List<ValidationFailure>>>;