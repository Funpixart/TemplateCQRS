using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Validators;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Payload<Unit, List<ValidationFailure>>>
{
    private readonly DeleteRoleCommandValidator _validator;
    private readonly RoleManager<Role> _roleManager;

    public DeleteRoleCommandHandler(DeleteRoleCommandValidator validator, RoleManager<Role> roleManager)
    {
        _validator = validator;
        _roleManager = roleManager;
    }

    public async Task<Payload<Unit, List<ValidationFailure>>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            var validationResult = new ValidationResult();

            // First find the role with the Id
            var roleFound = await _roleManager.FindByIdAsync(request.Id.ToString());

            if (roleFound is null)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "Role not found with the id provided."
                });
            }

            // Validate the request.
            validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;


            if (roleFound!.IsSystemRole)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status406NotAcceptable.ToString(),
                    ErrorMessage = "Cannot delete a System Role."
                });
            }
            else
            {
                // Delete the model.
                var result = await _roleManager.DeleteAsync(roleFound);

                // Add any error on creating the model.
                if (result.Errors.Any())
                {
                    validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);
                }
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