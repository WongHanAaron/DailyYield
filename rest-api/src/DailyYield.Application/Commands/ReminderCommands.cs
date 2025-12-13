using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Commands;

public class CreateReminderCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime ScheduledAt { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
}

public class UpdateReminderCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime ScheduledAt { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public ReminderStatus Status { get; set; } = ReminderStatus.Pending;
}

public class DeleteReminderCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}