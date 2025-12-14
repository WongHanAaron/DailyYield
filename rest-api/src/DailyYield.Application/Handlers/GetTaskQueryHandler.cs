using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskDto>
{
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;
    private readonly IMapper _mapper;

    public GetTaskQueryHandler(
        IRepository<TaskEntity> taskRepository,
        IRepository<UserGroupMember> memberRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
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

        return _mapper.Map<TaskDto>(task);
    }
}