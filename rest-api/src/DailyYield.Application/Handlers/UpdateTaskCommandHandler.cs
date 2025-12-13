using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly IRepository<TaskEntity> _taskRepository;

    public UpdateTaskCommandHandler(IRepository<TaskEntity> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        if (task.OwnerId != request.OwnerId)
        {
            throw new UnauthorizedAccessException("User does not own this task");
        }

        task.Title = request.Title;
        task.Status = request.Status;

        await _taskRepository.UpdateAsync(task);
    }
}