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

    public GetTaskQueryHandler(
        IRepository<TaskEntity> taskRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _taskRepository = taskRepository;
        _memberRepository = memberRepository;
    }

    public async Task<TaskDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        // Check access
        var hasAccess = task.UserId == request.UserId;
        if (!hasAccess && task.UserGroupId.HasValue)
        {
            var memberships = await _memberRepository.GetAllAsync();
            hasAccess = memberships.Any(m => m.UserId == request.UserId && m.UserGroupId == task.UserGroupId.Value);
        }

        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User does not have access to this task");
        }

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            UserId = task.UserId,
            UserGroupId = task.UserGroupId,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate
        };
    }
}