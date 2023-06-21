using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Application.Features.UserFeature.Validators;

namespace TemplateCQRS.Application.Features.UserFeature.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Payload<Unit, List<ValidationFailure>>>
{
    private readonly DeleteUserCommandValidator _validator;
    private readonly UserManager<User> _userManager;

    public DeleteUserCommandHandler(DeleteUserCommandValidator validator, UserManager<User> userManager)
    {
        _validator = validator;
        _userManager = userManager;
    }

    public async Task<Payload<Unit, List<ValidationFailure>>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            var userFound = await _userManager.FindByIdAsync(request.Id.ToString());

            if (userFound is not null)
            {
                var result = await _userManager.DeleteAsync(userFound);

                // Add any error on creating the model.
                if (result.Errors.Any())
                {
                    validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);
                }
            }
            else
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "User not found with the id provided."
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