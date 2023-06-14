using AutoMapper;
using FluentValidation.Results;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Validators;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TemplateCQRS.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Payload<Unit, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly DeleteRoleCommandValidator _validator;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IMapper mapper, DeleteRoleCommandValidator validator, IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
    {
        _mapper = mapper;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task<Payload<Unit, List<ValidationFailure>>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // Get the model by the Id
            var role = await _unitOfWork.FindByKey<Role>(request.Id);

            if (role is not null)
            {
                if (role.IsSystemRole)
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
                    var result = await _roleManager.DeleteAsync(role);

                    // Add any error on creating the model.
                    validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);
                }
            }
            else
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "Role not found or was deleted already."
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