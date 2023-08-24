using Microsoft.AspNetCore.OutputCaching;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Application.Features.ClaimFeature.Validators;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;

namespace TemplateCQRS.Application.Features.ClaimFeature.Handlers;

public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, Payload<Unit, List<ValidationFailure>>>
{
    private readonly DeleteClaimCommandValidator _validator;
    private readonly IRepository<RoleClaim> _repository;
    private readonly IOutputCacheStore _outputCacheStore;


    public DeleteClaimCommandHandler(DeleteClaimCommandValidator validator, IRepository<RoleClaim> repository, IOutputCacheStore outputCacheStore)
    {
        _validator = validator;
        _repository = repository;
        _outputCacheStore = outputCacheStore;
    }

    public async Task<Payload<Unit, List<ValidationFailure>>> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            // Get the model by the Id
            var claim = await _repository.GetByIdAsync(request.Id);

            if (claim is not null)
            {
                // Delete the model.
                await _repository.DeleteAsync(request.Id);

                // Refresh cache for the data modified.
                await _outputCacheStore.EvictByTagAsync(CachePolicy.GetClaims.Tag, cancellationToken);
            }
            else
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "Claim not found or was deleted already."
                });
            }

            // If there were any validation errors, return a failure payload, otherwise, success.
            return validationResult.Errors.Count > 0 ? validationResult.Errors : Unit.Value;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<DeleteRoleCommandHandler>(ex);
        }
    }
}
