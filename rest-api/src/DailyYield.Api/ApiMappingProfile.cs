using AutoMapper;
using DailyYield.Api.Controllers;
using DailyYield.Application.Commands;
using DailyYield.Application.Queries;

namespace DailyYield.Api;

/// <summary>
/// AutoMapper profile for mapping API request DTOs to domain commands and queries
/// </summary>
public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // Reminder mappings
        CreateMap<CreateReminderRequest, CreateReminderCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Set in controller

        CreateMap<UpdateReminderRequest, UpdateReminderCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Set in controller
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Set in controller

        // Task mappings
        CreateMap<CreateTaskRequest, CreateTaskCommand>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore()); // Set in controller

        CreateMap<UpdateTaskRequest, UpdateTaskCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Set in controller
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore()); // Set in controller

        // Goal mappings
        CreateMap<CreateGoalRequest, CreateGoalCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Set in controller

        CreateMap<UpdateGoalRequest, UpdateGoalCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Set in controller
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Set in controller

        // MetricType mappings
        CreateMap<CreateMetricTypeRequest, CreateMetricTypeCommand>();

        CreateMap<UpdateMetricTypeRequest, UpdateMetricTypeCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Set in controller

        // MetricEntry mappings
        CreateMap<CreateMetricEntryRequest, CreateMetricEntryCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Set in controller

        CreateMap<UpdateMetricEntryRequest, UpdateMetricEntryCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Set in controller
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Set in controller

        // UserGroup mappings
        CreateMap<CreateUserGroupRequest, CreateUserGroupCommand>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore()); // Set in controller
    }
}