using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class StartTaskTimerCommandHandler : IRequestHandler<StartTaskTimerCommand, Guid>
{
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<TaskTimer> _timerRepository;

    public StartTaskTimerCommandHandler(
        IRepository<TaskEntity> taskRepository,
        IRepository<TaskTimer> timerRepository)
    {
        _taskRepository = taskRepository;
        _timerRepository = timerRepository;
    }

    public async Task<Guid> Handle(StartTaskTimerCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        if (task.OwnerId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not own this task");
        }

        var timer = new TaskTimer
        {
            TaskId = request.TaskId,
            StartedAt = DateTime.UtcNow
        };

        await _timerRepository.AddAsync(timer);
        return timer.Id;
    }
}