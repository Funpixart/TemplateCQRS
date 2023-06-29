using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.UserFeature.Queries;

namespace TemplateCQRS.Application.Features.UserFeature.Handlers;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Payload<List<InfoUserDto>, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IRepository<RoleClaim> _claimRepository;
    private readonly IRepository<UserRole> _userRoleRepository;

    public GetAllUserQueryHandler(IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager,
        IRepository<RoleClaim> claimRepository,
        IRepository<UserRole> userRoleRepository)
    {
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _claimRepository = claimRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<Payload<List<InfoUserDto>, List<ValidationFailure>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        try
        {
            var users = _userManager.Users.ToList();
            if (!users.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            var roles = _roleManager.Roles.ToList();
            var claims = await _claimRepository.GetAllAsync(cancellationToken);
            var userRoles = await _userRoleRepository.GetAllAsync(cancellationToken);

            var infoUserDtos = new List<InfoUserDto>();

            foreach (var user in users)
            {
                var infoUserDto = new InfoUserDto();
                _ = _mapper.Map(user, infoUserDto);
                infoUserDto.RolesDto = new List<InfoRoleDto>();

                // Get all roles for the current user
                var userRoleIds = userRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId);

                foreach (var role in roles)
                {
                    // This user does not have this role, so we skip to the next role
                    if (!userRoleIds.Contains(role.Id)) continue;

                    var roleDto = new InfoRoleDto();
                    var claimList = claims.Where(x => x.RoleId == role.Id).Select(x => x.ClaimType).ToList();

                    _ = _mapper.Map(role, roleDto);

                    roleDto.Claims = claimList;
                    infoUserDto.RolesDto.Add(roleDto);
                }

                infoUserDtos.Add(infoUserDto);
            }

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : infoUserDtos;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetAllUserQueryHandler>(ex);
        }
    }
}