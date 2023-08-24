using TemplateCQRS.Application.Features.ClaimFeature.Queries;
using TemplateCQRS.Infrastructure.Data;

namespace TemplateCQRS.Application.Features.ClaimFeature.Handlers;

public class GetAllClaimByRoleQueryHandler : IRequestHandler<GetAllClaimsByRoleQuery, Payload<List<InfoClaimDto>, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<RoleClaim> _claimRepository;

    public GetAllClaimByRoleQueryHandler(IMapper mapper, IRepository<RoleClaim> claimRepository)
    {
        _mapper = mapper;
        _claimRepository = claimRepository;
    }

    public async Task<Payload<List<InfoClaimDto>, List<ValidationFailure>>> Handle(GetAllClaimsByRoleQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        try
        {
            var result = await _claimRepository.GetAllAsync(cancellationToken);
            if (!result.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            var claimMapped = _mapper.Map<List<InfoClaimDto>>(result.Where(x => x.RoleId == request.roleId));
            if (claimMapped is null)
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "claims wasn't able to be mapped."
                });
            }

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : claimMapped!;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetAllClaimQueryHandler>(ex);
        }
    }
}
