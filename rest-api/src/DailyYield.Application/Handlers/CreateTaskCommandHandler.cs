using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IRepository<TaskEntity> _taskRepository;

    public CreateTaskCommandHandler(IRepository<TaskEntity> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            UserGroupId = request.UserGroupId,
            DueDate = request.DueDate
        };

        await _taskRepository.AddAsync(task);
        return task.Id;
    }
}