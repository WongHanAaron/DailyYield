using AutoMapper;
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
    private readonly IMapper _mapper;

    public GetTaskCollaboratorsQueryHandler(
        IRepository<TaskCollaborator> collaboratorRepository,
        IRepository<TaskEntity> taskRepository,
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _collaboratorRepository = collaboratorRepository;
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskCollaboratorDto>> Handle(GetTaskCollaboratorsQuery request, CancellationToken cancellationToken)
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

        var collaborators = await _collaboratorRepository.GetAllAsync();
        var taskCollaborators = collaborators.Where(c => c.TaskId == request.TaskId);

        return _mapper.Map<IEnumerable<TaskCollaboratorDto>>(taskCollaborators);
    }
}