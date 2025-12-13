using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetGoalsQueryHandler : IRequestHandler<GetGoalsQuery, IEnumerable<GoalDto>>
{
    private readonly IRepository<Goal> _goalRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetGoalsQueryHandler(
        IRepository<Goal> goalRepository,
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _goalRepository = goalRepository;
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<GoalDto>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await _goalRepository.GetAllAsync();
        var userGoals = goals.Where(g => g.UserId == request.UserId);

        // Also include goals from user groups the user is member of
        var memberships = await _memberRepository.GetAllAsync();
        var userGroupIds = memberships.Where(m => m.UserId == request.UserId).Select(m => m.UserGroupId).ToList();
        var groupGoals = goals.Where(g => g.UserGroupId.HasValue && userGroupIds.Contains(g.UserGroupId.Value));

        var allGoals = userGoals.Concat(groupGoals);

        if (request.MetricTypeId.HasValue)
        {
            allGoals = allGoals.Where(g => g.MetricTypeId == request.MetricTypeId.Value);
        }

        var metricTypes = await _metricTypeRepository.GetAllAsync();
        var metricTypeDict = metricTypes.ToDictionary(mt => mt.Id, mt => (mt.Key, mt.DisplayName));

        return allGoals.Select(g => new GoalDto
        {
            Id = g.Id,
            MetricTypeId = g.MetricTypeId,
            MetricTypeKey = metricTypeDict.TryGetValue(g.MetricTypeId, out var mt) ? mt.Key : string.Empty,
            MetricTypeDisplayName = metricTypeDict.TryGetValue(g.MetricTypeId, out mt) ? mt.DisplayName : string.Empty,
            UserId = g.UserId,
            UserGroupId = g.UserGroupId,
            TargetValue = g.TargetValue,
            TimeframeStart = g.TimeframeStart,
            TimeframeEnd = g.TimeframeEnd,
            GoalType = g.GoalType,
            Frequency = g.Frequency,
            Comparison = g.Comparison,
            Status = g.Status,
            CreatedAt = g.CreatedAt,
            UpdatedAt = g.UpdatedAt
        });
    }
}