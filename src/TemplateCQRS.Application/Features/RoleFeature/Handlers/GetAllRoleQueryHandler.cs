using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TemplateCQRS.Application.Features.RoleFeature.Queries;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, Payload<List<InfoRoleDto>, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;
    private readonly IRepository<RoleClaim> _claimRepository;

    public GetAllRoleQueryHandler(IMapper mapper, RoleManager<Role> roleManager, IRepository<RoleClaim> roleClaims)
    {
        _mapper = mapper;
        _roleManager = roleManager;
        _claimRepository = roleClaims;
    }

    public async Task<Payload<List<InfoRoleDto>, List<ValidationFailure>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        try
        {
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);

            if (!roles.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }
            var roleDtos = new List<InfoRoleDto>();
            var claims = await _claimRepository.GetAllAsync(cancellationToken);

            foreach (var role in roles)
            {
                var roleDto = _mapper.Map<InfoRoleDto>(role);

                roleDto.Claims = claims.Where(x => x.RoleId == role.Id).Select(x => x.ClaimType).ToList();
                roleDtos.Add(roleDto);
            }

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : roleDtos;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetAllRoleQueryHandler>(ex);
        }
    }
}