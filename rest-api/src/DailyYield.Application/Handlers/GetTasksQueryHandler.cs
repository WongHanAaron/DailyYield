using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, IEnumerable<TaskDto>>
{
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetTasksQueryHandler(
        IRepository<TaskEntity> taskRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _taskRepository = taskRepository;
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetAllAsync();
        var userTasks = tasks.Where(t => t.UserId == request.UserId);

        // Also include tasks from user groups the user is member of
        var memberships = await _memberRepository.GetAllAsync();
        var userGroupIds = memberships.Where(m => m.UserId == request.UserId).Select(m => m.UserGroupId).ToList();
        var groupTasks = tasks.Where(t => t.UserGroupId.HasValue && userGroupIds.Contains(t.UserGroupId.Value));

        var allTasks = userTasks.Concat(groupTasks);

        if (request.IsCompleted.HasValue)
        {
            allTasks = allTasks.Where(t => t.IsCompleted == request.IsCompleted.Value);
        }

        return allTasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            UserId = t.UserId,
            UserGroupId = t.UserGroupId,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CreatedAt,
            DueDate = t.DueDate
        });
    }
}