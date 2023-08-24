using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Validators;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Payload<InfoRoleDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UpdateRoleCommandValidator _validator;
    private readonly RoleManager<Role> _roleManager;
    private readonly IRepository<RoleClaim> _claimRepository;
    private readonly IOutputCacheStore _outputCacheStore;

    public UpdateRoleCommandHandler(IMapper mapper, UpdateRoleCommandValidator validator, RoleManager<Role> roleManager,
        IRepository<RoleClaim> claimRepository, IOutputCacheStore outputCacheStore)
    {
        _mapper = mapper;
        _validator = validator;
        _roleManager = roleManager;
        _claimRepository = claimRepository;
        _outputCacheStore = outputCacheStore;
    }

    public async Task<Payload<InfoRoleDto, List<ValidationFailure>>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // First find the role with the Id
            var roleFound = _roleManager.Roles.FirstOrDefault(x => x.Id == request.Id);

            if (roleFound is null)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "Role not found with the id provided."
                });
            }

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            // Map the dto to the model. The _ means discard it since it will not be used.
            _ = _mapper.Map(request.UpdateRoleDto, roleFound);

            if (roleFound!.IsSystemRole)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status406NotAcceptable.ToString(),
                    ErrorMessage = "Cannot update a System Role."
                });
            }
            else
            {
                var result = await _roleManager.UpdateAsync(roleFound);

                // Refresh cache for new data.
                await _outputCacheStore.EvictByTagAsync(CachePolicy.GetRoles.Tag, cancellationToken);

                // Add any error on creating the model.
                if (result.Errors.Any())
                {
                    validationResult.Errors.AddIdentityErrorsToValidationFailures(result.Errors);
                }
            }

            var claims = await _claimRepository.GetAllAsync(cancellationToken);
            var roleDto = new InfoRoleDto();

            _ = _mapper.Map(roleFound, roleDto);

            if (claims.Any())
            {
                roleDto.Claims = claims.Where(x => x.RoleId == roleFound.Id).Select(x => x.ClaimType).ToList();
            }

            // If there were any validation errors, return a failure payload.
            return validationResult.Errors.Count > 0 ? validationResult.Errors : roleDto;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<UpdateRoleCommandHandler>(ex);
        }
    }
}