using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateCQRS.Application.Features.RoleFeature;

/// <summary>
///     Defines mapping configuration for Role entities using AutoMapper.
///     This class is instantiated by reflection during the application's initialization.
/// </summary>
public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        try
        {
            CreateMap<Role, RoleDto>().ReverseMap();
        }
        catch (Exception ex)
        {
            var msg = $"Something went wrong mapping, verify if the any navigation property was null. {ex.Message}";

            Log.Logger.Error(msg);
            throw new Exception(msg);
        }
    }
}