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

        // Also include reminders from user groups the user is member of
        var memberships = await _memberRepository.GetAllAsync();
        var userGroupIds = memberships.Where(m => m.UserId == request.UserId).Select(m => m.UserGroupId).ToList();
        var groupReminders = reminders.Where(r => r.UserGroupId.HasValue && userGroupIds.Contains(r.UserGroupId.Value));

        var allReminders = userReminders.Concat(groupReminders);

        if (request.IsActive.HasValue)
        {
            allReminders = allReminders.Where(r => r.IsActive == request.IsActive.Value);
        }

        var tasks = await _taskRepository.GetAllAsync();
        var taskDict = tasks.ToDictionary(t => t.Id, t => t.Title);

        var metricTypes = await _metricTypeRepository.GetAllAsync();
        var metricTypeDict = metricTypes.ToDictionary(mt => mt.Id, mt => mt.DisplayName);

        return allReminders.Select(r => new ReminderDto
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            UserId = r.UserId,
            UserGroupId = r.UserGroupId,
            TaskId = r.TaskId,
            TaskTitle = r.TaskId.HasValue && taskDict.TryGetValue(r.TaskId.Value, out var taskTitle) ? taskTitle : null,
            MetricTypeId = r.MetricTypeId,
            MetricTypeDisplayName = r.MetricTypeId.HasValue && metricTypeDict.TryGetValue(r.MetricTypeId.Value, out var mtName) ? mtName : null,
            ScheduleType = r.ScheduleType,
            Schedule = r.Schedule,
            IsActive = r.IsActive,
            CreatedAt = r.CreatedAt
        });
    }
}