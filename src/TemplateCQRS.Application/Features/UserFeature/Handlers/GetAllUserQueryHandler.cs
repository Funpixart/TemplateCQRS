using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            if (!users.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);

            var infoUserDtos = new List<InfoUserDto>();

            foreach (var user in users)
            {
                var infoUserDto = new InfoUserDto();
                _ = _mapper.Map(user, infoUserDto);
                infoUserDto.RolesDto = new List<InfoRoleClaimDto>();

                // Get all roles for the current user
                var userRoleIds = await _userRoleRepository.GetAllAsync(ur => ur.UserId == user.Id, cancellationToken);

                foreach (var role in roles)
                {
                    // This user does not have this role, so we skip to the next role
                    if (!userRoleIds.Select(ur => ur.RoleId).Contains(role.Id)) continue;

                    var roleDto = new InfoRoleClaimDto();
                    var claimList = await _claimRepository.GetAllAsync(x => x.RoleId == role.Id, cancellationToken);

                    _ = _mapper.Map(role, roleDto);

                    roleDto.Claims = claimList.Select(x => x.ClaimType).ToList();
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