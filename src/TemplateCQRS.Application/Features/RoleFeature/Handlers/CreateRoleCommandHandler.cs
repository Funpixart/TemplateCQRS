using AutoMapper;
using FluentValidation.Results;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TemplateCQRS.Application.Features.RoleFeature.Validators;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Payload<RoleDto, List<ValidationFailure>>>
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

    public async Task<Payload<RoleDto, List<ValidationFailure>>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // Map the dto to the model.
            var role = _mapper.Map<Role>(request.CreateRoleDto);

            // Create the model.
            var result = await _roleManager.CreateAsync(role);

            // Add any error on creating the model.
            validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0)
            {
                return Payload<RoleDto, List<ValidationFailure>>.Failure(validationResult.Errors);
            }

            var detailRoleDto = _mapper.Map<RoleDto>(role);
            var payload = Payload<RoleDto, List<ValidationFailure>>.Success(detailRoleDto);

            return payload;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<CreateRoleCommandHandler>(ex);
        }
    }
}