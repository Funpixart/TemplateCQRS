using AutoMapper;
using TemplateCQRS.Domain.Dto.Claim;

namespace TemplateCQRS.Application.Features.ClaimFeature;

public class ClaimMappingProfile : Profile
{
    public ClaimMappingProfile()
    {
        try
        {
            CreateMap<RoleClaim, CreateClaimDto>().ReverseMap();
            CreateMap<RoleClaim, InfoClaimDto>().ReverseMap();
            CreateMap<RoleClaim, UpdateClaimDto>().ReverseMap();
        }
        catch (Exception ex)
        {
            var msg = $"Something went wrong mapping, verify if the any navigation property was null. {ex.Message}";

            Log.Logger.Error(msg);
            throw new Exception(msg);
        }
    }
}