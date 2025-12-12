using MediatR;

namespace DailyYield.Application.Commands;

public class CreateTaskCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? UserGroupId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}

public class DeleteTaskCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class StartTaskTimerCommand : IRequest<Guid>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

public class StopTaskTimerCommand : IRequest
{
    public Guid TimerId { get; set; }
    public Guid UserId { get; set; }
}

public class AddTaskCollaboratorCommand : IRequest
{
    public Guid TaskId { get; set; }
    public Guid CollaboratorUserId { get; set; }
    public Guid OwnerUserId { get; set; }
}

public class RemoveTaskCollaboratorCommand : IRequest
{
    public Guid TaskId { get; set; }
    public Guid CollaboratorUserId { get; set; }
    public Guid OwnerUserId { get; set; }
}