using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, IEnumerable<ReminderDto>>
{
    private readonly IRepository<Reminder> _reminderRepository;
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetRemindersQueryHandler(
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

    public async Task<IEnumerable<ReminderDto>> Handle(GetRemindersQuery request, CancellationToken cancellationToken)
    {
        var reminders = await _reminderRepository.GetAllAsync();
        var userReminders = reminders.Where(r => r.UserId == request.UserId);

        if (request.Status.HasValue)
        {
            userReminders = userReminders.Where(r => r.Status == request.Status.Value);
        }

        return userReminders.Select(r => new ReminderDto
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            UserId = r.UserId,
            ScheduledAt = r.ScheduledAt,
            IsRecurring = r.IsRecurring,
            RecurrencePattern = r.RecurrencePattern,
            Status = r.Status,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt
        });
    }
}