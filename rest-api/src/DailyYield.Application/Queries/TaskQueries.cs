using MediatR;
using DailyYield.Domain.Entities;

namespace DailyYield.Application.Queries;

public class GetTasksQuery : IRequest<IEnumerable<TaskDto>>
{
    public Guid UserId { get; set; }
    public DailyYield.Domain.Entities.TaskStatus? Status { get; set; }
}

public class GetTaskQuery : IRequest<TaskDto>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class GetTaskTimersQuery : IRequest<IEnumerable<TaskTimerDto>>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

public class GetTaskCollaboratorsQuery : IRequest<IEnumerable<TaskCollaboratorDto>>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public Guid? CategoryId { get; set; }
    public DailyYield.Domain.Entities.TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TaskTimerDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public TimeSpan? Duration { get; set; }
}

public class TaskCollaboratorDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; }
}