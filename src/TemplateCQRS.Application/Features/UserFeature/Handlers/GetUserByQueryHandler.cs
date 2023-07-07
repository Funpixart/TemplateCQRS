using AutoMapper;
using TemplateCQRS.Application.Features.UserFeature.Queries;
using TemplateCQRS.Application.Features.UserFeature.Validators;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TemplateCQRS.Application.Features.UserFeature.Handlers;

public class GetUserByQueryHandler : IRequestHandler<GetUserByQuery, Payload<InfoUserDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IRepository<RoleClaim> _claimRepository;
    private readonly GetUserByQueryValidator _validator;

    public GetUserByQueryHandler(IMapper mapper, UserManager<User> userManager, GetUserByQueryValidator validator, RoleManager<Role> roleManager, IRepository<RoleClaim> claimRepository)
    {
        _mapper = mapper;
        _userManager = userManager;
        _validator = validator;
        _roleManager = roleManager;
        _claimRepository = claimRepository;
    }

    public async Task<Payload<InfoUserDto, List<ValidationFailure>>> Handle(GetUserByQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        var user = new User();
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            // Check if id is null
            if (request.GetUserDto.Id is null)
            {
                // Check if name is also null and return.
                if (!string.IsNullOrEmpty(request.GetUserDto.UserName))
                {
                    user = await _userManager.Users
                        .FirstOrDefaultAsync(x =>
                            !string.IsNullOrEmpty(x.UserName) &&
                            x.UserName.ToLower() == request.GetUserDto.UserName.ToLower(), cancellationToken);
                }
                else if (!string.IsNullOrEmpty(request.GetUserDto.Email))
                {
                    user = await _userManager.Users
                        .FirstOrDefaultAsync(x =>
                            !string.IsNullOrEmpty(x.Email) &&
                            x.Email.ToLower() == request.GetUserDto.Email.ToLower(), cancellationToken);
                }
                else
                {
                    errors.Add(new ValidationFailure
                    {
                        ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                        ErrorMessage = "Please provide a valid name, email or Id."
                    });
                    return errors;
                }
            }
            else
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.Id == request.GetUserDto.Id,cancellationToken);
            }
            
            if (user is null)
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            // Get all roles by the user
            var userRolesName = await _userManager.GetRolesAsync(user);
            var claims = await _claimRepository.GetAllAsync(cancellationToken);

            var infoUserDto = new InfoUserDto();
            _ = _mapper.Map(user, infoUserDto);
            infoUserDto.RolesDto = new List<InfoRoleClaimDto>();

            foreach (var role in _roleManager.Roles)
            {
                if (!userRolesName.Contains(role.Name)) continue;

                var roleDto = new InfoRoleClaimDto();
                var claimList = claims.Where(x => x.RoleId == role.Id).Select(x => x.ClaimType).ToList();

                _ = _mapper.Map(role, roleDto);

                roleDto.Claims = claimList;
                infoUserDto.RolesDto.Add(roleDto);
            }

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : infoUserDto;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetUserByQueryHandler>(ex);
        }
    }
}