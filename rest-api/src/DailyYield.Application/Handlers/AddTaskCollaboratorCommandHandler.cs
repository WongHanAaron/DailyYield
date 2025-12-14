using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class AddTaskCollaboratorCommandHandler : IRequestHandler<AddTaskCollaboratorCommand>
{
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<TaskCollaborator> _collaboratorRepository;

    public AddTaskCollaboratorCommandHandler(
        IRepository<TaskEntity> taskRepository,
        IRepository<TaskCollaborator> collaboratorRepository)
    {
        _taskRepository = taskRepository;
        _collaboratorRepository = collaboratorRepository;
    }

    public async System.Threading.Tasks.Task Handle(AddTaskCollaboratorCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        if (task.OwnerId != request.OwnerUserId)
        {
            throw new UnauthorizedAccessException("User does not own this task");
        }

        var collaborator = new TaskCollaborator
        {
            TaskId = request.TaskId,
            UserId = request.CollaboratorUserId
        };

        await _collaboratorRepository.AddAsync(collaborator);
    }
}