using TemplateCQRS.Domain.Dto.Claim;
using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.ClaimFeature.Queries;

public record GetAllClaimsQuery : IRequest<Payload<List<InfoClaimDto>, List<ValidationFailure>>>;
public record GetAllClaimsByRoleQuery(Guid roleId) : IRequest<Payload<List<InfoClaimDto>, List<ValidationFailure>>>;
