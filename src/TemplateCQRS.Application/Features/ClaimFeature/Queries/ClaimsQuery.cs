using FluentValidation.Results;
using MediatR;
using TemplateCQRS.Domain.Dto.Claim;

namespace TemplateCQRS.Application.Features.ClaimFeature.Queries;

public record GetAllClaimsQuery : IRequest<Payload<List<InfoClaimDto>, List<ValidationFailure>>>;
public record GetAllClaimsByRoleQuery(Guid roleId) : IRequest<Payload<List<InfoClaimDto>, List<ValidationFailure>>>;
