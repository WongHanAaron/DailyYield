using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetReminderQueryHandler : IRequestHandler<GetReminderQuery, ReminderDto>
{
    private readonly IRepository<Reminder> _reminderRepository;
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetReminderQueryHandler(
        IRepository<Reminder> reminderRepository,
        IRepository<TaskEntity> taskRepository,
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _reminderRepository = reminderRepository;
        _taskRepository = taskRepository;
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
    }

    public async Task<ReminderDto> Handle(GetReminderQuery request, CancellationToken cancellationToken)
    {
        var reminder = await _reminderRepository.GetByIdAsync(request.Id);
        if (reminder == null)
        {
            throw new KeyNotFoundException("Reminder not found");
        }

        // Check access
        var hasAccess = reminder.UserId == request.UserId;
        if (!hasAccess && reminder.UserGroupId.HasValue)
        {
            var memberships = await _memberRepository.GetAllAsync();
            hasAccess = memberships.Any(m => m.UserId == request.UserId && m.UserGroupId == reminder.UserGroupId.Value);
        }

        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User does not have access to this reminder");
        }

        var taskTitle = reminder.TaskId.HasValue ? (await _taskRepository.GetByIdAsync(reminder.TaskId.Value))?.Title : null;
        var metricTypeName = reminder.MetricTypeId.HasValue ? (await _metricTypeRepository.GetByIdAsync(reminder.MetricTypeId.Value))?.DisplayName : null;

        return new ReminderDto
        {
            Id = reminder.Id,
            Title = reminder.Title,
            Description = reminder.Description,
            UserId = reminder.UserId,
            UserGroupId = reminder.UserGroupId,
            TaskId = reminder.TaskId,
            TaskTitle = taskTitle,
            MetricTypeId = reminder.MetricTypeId,
            MetricTypeDisplayName = metricTypeName,
            ScheduleType = reminder.ScheduleType,
            Schedule = reminder.Schedule,
            IsActive = reminder.IsActive,
            CreatedAt = reminder.CreatedAt
        };
    }
}