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
        
        // Include tasks where user is owner or collaborator
        var userTasks = tasks.Where(t => t.OwnerId == request.UserId || 
                                        t.Collaborators.Any(c => c.UserId == request.UserId));

        if (request.Status.HasValue)
        {
            userTasks = userTasks.Where(t => t.Status == request.Status.Value);
        }

        return userTasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            OwnerId = t.OwnerId,
            CategoryId = t.CategoryId,
            Status = t.Status,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        });
    }
}