using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Application.Features.UserFeature.Validators;

namespace TemplateCQRS.Application.Features.UserFeature.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Payload<Unit, List<ValidationFailure>>>
{
    private readonly DeleteUserCommandValidator _validator;
    private readonly UserManager<User> _userManager;
    private readonly IOutputCacheStore _outputCacheStore;

    public DeleteUserCommandHandler(DeleteUserCommandValidator validator, UserManager<User> userManager, IOutputCacheStore outputCacheStore)
    {
        _validator = validator;
        _userManager = userManager;
        _outputCacheStore = outputCacheStore;
    }

    public async Task<Payload<Unit, List<ValidationFailure>>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            var userFound = await _userManager.FindByIdAsync(request.Id.ToString());

            if (userFound is null)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "User not found with the id provided."
                });
            }

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            var result = await _userManager.DeleteAsync(userFound!);

            // Refresh cache for the data modified.
            await _outputCacheStore.EvictByTagAsync(CachePolicy.GetUsers.Tag, cancellationToken);
            await _outputCacheStore.EvictByTagAsync(CachePolicy.GetUserBy.Tag, cancellationToken);

            // Add any error on creating the model.
            if (result.Errors.Any())
            {
                validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);
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