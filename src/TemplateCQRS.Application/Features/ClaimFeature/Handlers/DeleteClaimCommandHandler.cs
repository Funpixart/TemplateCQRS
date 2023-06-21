using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Application.Features.ClaimFeature.Validators;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;

namespace TemplateCQRS.Application.Features.ClaimFeature.Handlers;

public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, Payload<Unit, List<ValidationFailure>>>
{
    private readonly DeleteClaimCommandValidator _validator;
    private readonly IRepository<RoleClaim> _repo;


    public DeleteClaimCommandHandler(DeleteClaimCommandValidator validator, IRepository<RoleClaim> repo)
    {
        _validator = validator;
        _repo = repo;
    }

    public async Task<Payload<Unit, List<ValidationFailure>>> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // Get the model by the Id
            var claim = await _repo.GetByIdAsync(request.Id);

            if (claim is not null)
            {
                // Delete the model.
                await _repo.DeleteAsync(request.Id);
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
