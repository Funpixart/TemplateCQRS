﻿using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Validators;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Payload<InfoRoleDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly CreateRoleCommandValidator _validator;
    private readonly RoleManager<Role> _roleManager;

    public CreateRoleCommandHandler(IMapper mapper, CreateRoleCommandValidator validator, RoleManager<Role> roleManager)
    {
        _mapper = mapper;
        _validator = validator;
        _roleManager = roleManager;
    }

    public async Task<Payload<InfoRoleDto, List<ValidationFailure>>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            // Map the dto to the model.
            var role = _mapper.Map<Role>(request.CreateRoleDto);

            // Create the model.
            var result = await _roleManager.CreateAsync(role);

            // Add any error on creating the model.
            validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            return _mapper.Map<InfoRoleDto>(role);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<CreateRoleCommandHandler>(ex);
        }
    }
}