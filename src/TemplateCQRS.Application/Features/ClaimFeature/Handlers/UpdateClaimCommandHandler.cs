using Microsoft.AspNetCore.OutputCaching;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Application.Features.ClaimFeature.Validators;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;
using TemplateCQRS.Infrastructure.Data;

namespace TemplateCQRS.Application.Features.ClaimFeature.Handlers;

public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, Payload<InfoClaimDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UpdateClaimCommandValidator _validator;
    private readonly IRepository<RoleClaim> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutputCacheStore _outputCacheStore;

    public UpdateClaimCommandHandler(IMapper mapper, UpdateClaimCommandValidator validator, IRepository<RoleClaim> repository, IUnitOfWork unitOfWork, IOutputCacheStore outputCacheStore)
    {
        _mapper = mapper;
        _validator = validator;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _outputCacheStore = outputCacheStore;
    }

    public async Task<Payload<InfoClaimDto, List<ValidationFailure>>> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            // Get the model by the Id
            var claimFound = await _unitOfWork.FindByKey<RoleClaim>(request.Id);

            // Map the dto to the model.
            _mapper.Map(request.UpdateClaimDto, claimFound);
            if (claimFound is not null)
            {
                _ = await _repository.UpdateAsync(claimFound);

                // Refresh cache for new data.
                await _outputCacheStore.EvictByTagAsync(CachePolicy.GetClaims.Tag, cancellationToken);
            }
            else
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "claim not found with the id provided."
                });
            }

            // If there were any validation errors, return a failure payload.
            return validationResult.Errors.Count > 0 ? validationResult.Errors : _mapper.Map<InfoClaimDto>(claimFound);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<UpdateRoleCommandHandler>(ex);
        }
    }
}
