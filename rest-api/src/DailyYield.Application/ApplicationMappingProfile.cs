using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;

namespace DailyYield.Application;

/// <summary>
/// AutoMapper profile for mapping domain entities to application DTOs
/// </summary>
public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        // UserGroup mappings
        CreateMap<UserGroupMember, UserGroupDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserGroup.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserGroup.Name))
            .ForMember(dest => dest.Timezone, opt => opt.MapFrom(src => src.UserGroup.Timezone))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
    }
}