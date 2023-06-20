using TemplateCQRS.Domain.Dto.Claim;
using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.ClaimFeature.Commands;

public record CreateClaimCommand(CreateClaimDto CreateClaimDto) : IRequest<Payload<InfoClaimDto, List<ValidationFailure>>>;

public record UpdateClaimCommand(int Id, UpdateClaimDto UpdateClaimDto) : IRequest<Payload<InfoClaimDto, List<ValidationFailure>>>;

public record DeleteClaimCommand(int Id) : IRequest<Payload<Unit, List<ValidationFailure>>>;