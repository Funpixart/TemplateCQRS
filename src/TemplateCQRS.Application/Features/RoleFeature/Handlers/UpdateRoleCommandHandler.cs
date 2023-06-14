using AutoMapper;
using FluentValidation.Results;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Validators;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TemplateCQRS.Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Payload<RoleDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UpdateRoleCommandValidator _validator;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(IMapper mapper, UpdateRoleCommandValidator validator, RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _validator = validator;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Payload<RoleDto, List<ValidationFailure>>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // Map the dto to the model.
            var role = _mapper.Map<Role>(request.UpdateRoleDto);

            // Get the model by the Id
            var roleFound = await _unitOfWork.FindByKey<Role>(request.Id);

            if (roleFound is not null)
            {
                if (roleFound.IsSystemRole)
                {
                    validationResult.Errors.Add(new ValidationFailure
                    {
                        ErrorCode = StatusCodes.Status406NotAcceptable.ToString(),
                        ErrorMessage = "Cannot update a System Role."
                    });
                }
                else
                {
                    var result = await _roleManager.UpdateAsync(role);

                    // Add any error on creating the model.
                    if (result.Errors.Any())
                    {
                        validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);
                    }
                }
            }
            else
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "Role not found with the id provided."
                });
            }

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
            return ErrorHelper.LogExceptionAndReturnError<UpdateRoleCommandHandler>(ex);
        }
    }
}