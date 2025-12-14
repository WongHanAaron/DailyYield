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
    public Guid? UserId { get; set; }
    public Guid? UserGroupId { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? MetricTypeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public ReminderType ReminderType { get; set; } = ReminderType.OneTime;
    public string Schedule { get; set; } = string.Empty; // JSON: cron expression for recurring; date/time for one_time
    public bool IsActive { get; set; } = true;
    public DateTime? LastTriggered { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}