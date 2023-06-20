using AutoMapper;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Application.Features.UserFeature.Validators;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace TemplateCQRS.Application.Features.UserFeature.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Payload<InfoUserDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UpdateUserCommandValidator _validator;
    private readonly UserManager<User> _userManager;

    public UpdateUserCommandHandler(UpdateUserCommandValidator validator, UserManager<User> userManager, IMapper mapper)
    {
        _validator = validator;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Payload<InfoUserDto, List<ValidationFailure>>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            var validationResult = new ValidationResult();

            // First find the user.
            var userFound = await _userManager.FindByIdAsync(request.Id.ToString());

            if (userFound is null)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "User not found with the id provided."
                });
            }

            // Validate the request.
            validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            _mapper.Map(request.UpdateUserDto, userFound);

            var result = await _userManager.UpdateAsync(userFound!);

            // Add any error on creating the model.
            if (result.Errors.Any()) validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);

            // If there were any validation errors, return a failure payload, otherwise, success.
            return validationResult.Errors.Count > 0 ? validationResult.Errors : _mapper.Map<InfoUserDto>(userFound!);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<DeleteUserCommandHandler>(ex);
        }
    }
}