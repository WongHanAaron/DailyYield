using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Commands;

public class CreateReminderCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? UserGroupId { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? MetricTypeId { get; set; }
    public ReminderScheduleType ScheduleType { get; set; } = ReminderScheduleType.Simple;
    public string Schedule { get; set; } = string.Empty;
}

public class UpdateReminderCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? MetricTypeId { get; set; }
    public ReminderScheduleType ScheduleType { get; set; } = ReminderScheduleType.Simple;
    public string Schedule { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class DeleteReminderCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}