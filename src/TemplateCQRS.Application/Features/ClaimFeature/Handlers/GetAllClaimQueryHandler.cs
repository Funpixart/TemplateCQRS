using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using TemplateCQRS.Application.Features.ClaimFeature.Queries;
using TemplateCQRS.Domain.Dto.Claim;
using TemplateCQRS.Infrastructure.Data;

namespace TemplateCQRS.Application.Features.ClaimFeature.Handlers;

public class GetAllClaimQueryHandler : IRequestHandler<GetAllClaimsQuery, Payload<List<InfoClaimDto>, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<RoleClaim> _claimRepository;

    public GetAllClaimQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IRepository<RoleClaim> claimRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimRepository = claimRepository;
    }

    public async Task<Payload<List<InfoClaimDto>, List<ValidationFailure>>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        try
        {
            var result = await _claimRepository.GetAllAsync();
            if (!result.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            var claimMapped = _mapper.Map<List<InfoClaimDto>>(result);
            if (claimMapped is null)
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "claims wasn't able to be mapped."
                });
            }

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : Payload<List<InfoClaimDto>, List<ValidationFailure>>.Success(claimMapped!);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetAllClaimQueryHandler>(ex);
        }
    }
}
