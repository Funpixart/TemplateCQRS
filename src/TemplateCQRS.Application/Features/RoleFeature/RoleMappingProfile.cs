using AutoMapper;

namespace TemplateCQRS.Application.Features.RoleFeature;

/// <summary>
///     Defines mapping configuration for Role entities using AutoMapper.
///     This class is instantiated by reflection during the application's initialization.
/// </summary>
internal class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        try
        {
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            CreateMap<Role, InfoRoleDto>().ReverseMap();
            CreateMap<Role, InfoRoleClaimDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
        }
        catch (Exception ex)
        {
            var msg = $"Something went wrong mapping, verify if the any navigation property was null. {ex.Message}";

            Log.Logger.Error(msg);
            throw new Exception(msg);
        }
    }
}