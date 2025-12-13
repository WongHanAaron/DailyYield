using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetTaskTimersQueryHandler : IRequestHandler<GetTaskTimersQuery, IEnumerable<TaskTimerDto>>
{
    private readonly IRepository<TaskTimer> _timerRepository;
    private readonly IRepository<TaskEntity> _taskRepository;

    public GetTaskTimersQueryHandler(
        IRepository<TaskTimer> timerRepository,
        IRepository<TaskEntity> taskRepository)
    {
        _timerRepository = timerRepository;
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskTimerDto>> Handle(GetTaskTimersQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        // Check access - user must be owner or collaborator
        var hasAccess = task.OwnerId == request.UserId;
        if (!hasAccess)
        {
            hasAccess = task.Collaborators.Any(c => c.UserId == request.UserId);
        }

        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User does not have access to this task");
        }

        var timers = await _timerRepository.GetAllAsync();
        var taskTimers = timers.Where(t => t.TaskId == request.TaskId);

        return taskTimers.Select(t => new TaskTimerDto
        {
            Id = t.Id,
            TaskId = t.TaskId,
            StartedAt = t.StartedAt,
            EndedAt = t.EndedAt,
            Duration = t.Duration
        });
    }
}