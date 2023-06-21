using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.RoleFeature.Queries;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, Payload<List<InfoRoleDto>, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;

    public GetAllRoleQueryHandler(IMapper mapper, RoleManager<Role> roleManager)
    {
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<Payload<List<InfoRoleDto>, List<ValidationFailure>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        try
        {
            var result = _roleManager.Roles;
            if (!result.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }
            var roleMapped = _mapper.Map<List<InfoRoleDto>>(result.ToList());

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : roleMapped;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetAllRoleQueryHandler>(ex);
        }
    }
}