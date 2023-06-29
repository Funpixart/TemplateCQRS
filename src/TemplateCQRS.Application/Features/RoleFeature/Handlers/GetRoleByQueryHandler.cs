using AutoMapper;
using TemplateCQRS.Application.Features.RoleFeature.Queries;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class GetRoleByQueryHandler : IRequestHandler<GetRoleByQuery, Payload<InfoRoleDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;
    private readonly IRepository<RoleClaim> _claimRepository;

    public GetRoleByQueryHandler(IMapper mapper, RoleManager<Role> roleManager, IRepository<RoleClaim> claimRepository)
    {
        _mapper = mapper;
        _roleManager = roleManager;
        _claimRepository = claimRepository;
    }

    public async Task<Payload<InfoRoleDto, List<ValidationFailure>>> Handle(GetRoleByQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        var role = new Role();
        try
        {
            // Check if id is null
            if (request.Id is null || request.Id == Guid.Empty)
            {
                // Check if name is also null and return.
                if (string.IsNullOrEmpty(request.Name))
                {
                    errors.Add(new ValidationFailure
                    {
                        ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                        ErrorMessage = "Please provide a valid name or Id."
                    });
                    return errors;
                }

                role = await _roleManager.Roles.FirstOrDefaultAsync(x=> x.Name.ToLower() == request.Name.ToLower(), cancellationToken);
            }
            else
            {
                role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            }

            if (role is null)
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            var claims = await _claimRepository.GetAllAsync(cancellationToken);
            var roleDto = _mapper.Map<InfoRoleDto>(role);

            if (claims.Any())
            {
                roleDto.Claims = claims.Where(x => x.RoleId == role.Id).Select(x => x.ClaimType).ToList();
            }

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : roleDto;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetAllRoleQueryHandler>(ex);
        }
    }
}