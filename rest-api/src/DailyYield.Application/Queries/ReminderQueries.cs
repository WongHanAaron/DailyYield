using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Queries;

public class GetRemindersQuery : IRequest<IEnumerable<ReminderDto>>
{
    public Guid UserId { get; set; }
    public bool? IsActive { get; set; }
}

public class GetReminderQuery : IRequest<ReminderDto>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class ReminderDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? UserId { get; set; }
    public Guid? UserGroupId { get; set; }
    public Guid? TaskId { get; set; }
    public string? TaskTitle { get; set; }
    public Guid? MetricTypeId { get; set; }
    public string? MetricTypeDisplayName { get; set; }
    public ReminderScheduleType ScheduleType { get; set; } = ReminderScheduleType.Simple;
    public string Schedule { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}