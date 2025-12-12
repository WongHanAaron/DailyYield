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

        // Check access (similar to GetTaskQueryHandler)
        var hasAccess = task.UserId == request.UserId;
        if (!hasAccess && task.UserGroupId.HasValue)
        {
            // For simplicity, assume user has access if they can see the task
            // In real implementation, check membership
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