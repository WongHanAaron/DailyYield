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

        // Task mappings
        CreateMap<DailyYield.Domain.Entities.Task, TaskDto>();

        // TaskTimer mappings
        CreateMap<TaskTimer, TaskTimerDto>();

        // TaskCollaborator mappings
        CreateMap<TaskCollaborator, TaskCollaboratorDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.DisplayName));

        // Goal mappings (will be customized in handlers for MetricType data)
        CreateMap<Goal, GoalDto>()
            .ForMember(dest => dest.MetricTypeKey, opt => opt.Ignore())
            .ForMember(dest => dest.MetricTypeDisplayName, opt => opt.Ignore());

        // MetricEntry mappings (will be customized in handlers for MetricType data)
        CreateMap<MetricEntry, MetricEntryDto>()
            .ForMember(dest => dest.MetricTypeKey, opt => opt.Ignore())
            .ForMember(dest => dest.MetricTypeDisplayName, opt => opt.Ignore());

        // MetricType mappings
        CreateMap<MetricType, MetricTypeDto>();

        // Reminder mappings
        CreateMap<Reminder, ReminderDto>();

        // YieldSummary mappings
        CreateMap<YieldSummary, YieldSummaryDto>();
    }
}