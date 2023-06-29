using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Application.Features.UserFeature.Validators;
using TemplateCQRS.Domain.Common;

namespace TemplateCQRS.Application.Features.UserFeature.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Payload<InfoUserDto, List<ValidationFailure>>>
{
    private readonly UserManager<User> _userManager;
    private readonly CreateUserCommandValidator _validator;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(CreateUserCommandValidator validator, IMapper mapper, UserManager<User> userManager)
    {
        _validator = validator;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Payload<InfoUserDto, List<ValidationFailure>>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            // Map the dto to the model.
            var user = _mapper.Map<User>(request.CreateUserDto);

            // Create the user.
            var userResult = await _userManager.CreateAsync(user, request.CreateUserDto.Password);
            if (userResult.Succeeded)
            {
                // Add a role to the user.
                var userToRoleResult = await _userManager.AddToRoleAsync(user, Constants.DefaultRoles.Visitor.Name);
                if (!userToRoleResult.Succeeded)
                {
                    // Add any error on adding the role to the user.
                    validationResult.Errors.AddIdentityErrorsToValidationFailures(userToRoleResult.Errors);
                }
            }
            else if (userResult.Errors.Any())
            {
                // Add any error on creating the user.
                validationResult.Errors.AddIdentityErrorsToValidationFailures(userResult.Errors);
            }

            var mappedUser = _mapper.Map<InfoUserDto>(user);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            return mappedUser;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<CreateUserCommandHandler>(ex);
        }
    }
}