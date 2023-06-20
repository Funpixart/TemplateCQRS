using AutoMapper;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Validators;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Validators;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Payload<InfoRoleDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UpdateRoleCommandValidator _validator;
    private readonly RoleManager<Role> _roleManager;

    public UpdateRoleCommandHandler(IMapper mapper, UpdateRoleCommandValidator validator, RoleManager<Role> roleManager)
    {
        _mapper = mapper;
        _validator = validator;
        _roleManager = roleManager;
    }

    public async Task<Payload<InfoRoleDto, List<ValidationFailure>>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
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

            // Map the dto to the model.
            _mapper.Map(request.UpdateRoleDto, roleFound);

            if (roleFound!.IsSystemRole)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status406NotAcceptable.ToString(),
                    ErrorMessage = "Cannot update a System Role."
                });
            }
            else
            {
                var result = await _roleManager.UpdateAsync(roleFound);

                // Add any error on creating the model.
                if (result.Errors.Any())
                {
                    validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);
                }
            }

            // If there were any validation errors, return a failure payload.
            return validationResult.Errors.Count > 0 ? validationResult.Errors : _mapper.Map<InfoRoleDto>(roleFound);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<UpdateRoleCommandHandler>(ex);
        }
    }
}