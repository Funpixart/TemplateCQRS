using AutoMapper;

namespace TemplateCQRS.Application.Features.UserFeature;

internal class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        try
        {
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, InfoUserDto>().ReverseMap();
        }
        catch (Exception ex)
        {
            var msg = $"Something went wrong mapping, verify if the any navigation property was null. {ex.Message}";

            Log.Logger.Error(msg);
            throw new Exception(msg);
        }
    }
}