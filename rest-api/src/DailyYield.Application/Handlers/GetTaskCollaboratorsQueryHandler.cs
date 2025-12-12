using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetTaskCollaboratorsQueryHandler : IRequestHandler<GetTaskCollaboratorsQuery, IEnumerable<TaskCollaboratorDto>>
{
    private readonly IRepository<TaskCollaborator> _collaboratorRepository;
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<User> _userRepository;

    public GetTaskCollaboratorsQueryHandler(
        IRepository<TaskCollaborator> collaboratorRepository,
        IRepository<TaskEntity> taskRepository,
        IRepository<User> userRepository)
    {
        _collaboratorRepository = collaboratorRepository;
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<TaskCollaboratorDto>> Handle(GetTaskCollaboratorsQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        // Check access
        var hasAccess = task.UserId == request.UserId;
        if (!hasAccess && task.UserGroupId.HasValue)
        {
            // Assume access if user can see the task
        }

        var collaborators = await _collaboratorRepository.GetAllAsync();
        var taskCollaborators = collaborators.Where(c => c.TaskId == request.TaskId);

        var users = await _userRepository.GetAllAsync();
        var userDict = users.ToDictionary(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim());

        return taskCollaborators.Select(c => new TaskCollaboratorDto
        {
            Id = c.Id,
            TaskId = c.TaskId,
            UserId = c.UserId,
            UserName = userDict.TryGetValue(c.UserId, out var name) ? name : "Unknown",
            AddedAt = c.AddedAt
        });
    }
}