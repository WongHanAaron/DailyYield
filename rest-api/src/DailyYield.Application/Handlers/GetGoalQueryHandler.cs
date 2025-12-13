using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetGoalQueryHandler : IRequestHandler<GetGoalQuery, GoalDto>
{
    private readonly IRepository<Goal> _goalRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetGoalQueryHandler(
        IRepository<Goal> goalRepository,
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _goalRepository = goalRepository;
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
    }

    public async Task<GoalDto> Handle(GetGoalQuery request, CancellationToken cancellationToken)
    {
        var goal = await _goalRepository.GetByIdAsync(request.Id);
        if (goal == null)
        {
            throw new KeyNotFoundException("Goal not found");
        }

        // Check access
        var hasAccess = goal.UserId == request.UserId;
        if (!hasAccess && goal.UserGroupId.HasValue)
        {
            var memberships = await _memberRepository.GetAllAsync();
            hasAccess = memberships.Any(m => m.UserId == request.UserId && m.UserGroupId == goal.UserGroupId.Value);
        }

        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User does not have access to this goal");
        }

        var metricType = await _metricTypeRepository.GetByIdAsync(goal.MetricTypeId);

        return new GoalDto
        {
            Id = goal.Id,
            MetricTypeId = goal.MetricTypeId,
            MetricTypeKey = metricType?.Key ?? string.Empty,
            MetricTypeDisplayName = metricType?.DisplayName ?? string.Empty,
            UserId = goal.UserId,
            UserGroupId = goal.UserGroupId,
            TargetValue = goal.TargetValue,
            TimeframeStart = goal.TimeframeStart,
            TimeframeEnd = goal.TimeframeEnd,
            GoalType = goal.GoalType,
            Frequency = goal.Frequency,
            Comparison = goal.Comparison,
            Status = goal.Status,
            CreatedAt = goal.CreatedAt,
            UpdatedAt = goal.UpdatedAt
        };
    }
}