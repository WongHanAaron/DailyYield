using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly IRepository<TaskEntity> _taskRepository;

    public DeleteTaskCommandHandler(IRepository<TaskEntity> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        if (task.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not own this task");
        }

        await _taskRepository.DeleteAsync(task);
    }
}