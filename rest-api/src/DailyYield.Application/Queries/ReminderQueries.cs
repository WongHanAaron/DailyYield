using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Queries;

public class GetRemindersQuery : IRequest<IEnumerable<ReminderDto>>
{
    public Guid UserId { get; set; }
    public ReminderStatus? Status { get; set; }
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
    public DateTime ScheduledAt { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public ReminderStatus Status { get; set; } = ReminderStatus.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}