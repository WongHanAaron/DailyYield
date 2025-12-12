using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetYieldSummariesQueryHandler : IRequestHandler<GetYieldSummariesQuery, IEnumerable<YieldSummaryDto>>
{
    private readonly IRepository<YieldSummary> _yieldSummaryRepository;
    private readonly IRepository<UserGroup> _userGroupRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetYieldSummariesQueryHandler(
        IRepository<YieldSummary> yieldSummaryRepository,
        IRepository<UserGroup> userGroupRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _yieldSummaryRepository = yieldSummaryRepository;
        _userGroupRepository = userGroupRepository;
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<YieldSummaryDto>> Handle(GetYieldSummariesQuery request, CancellationToken cancellationToken)
    {
        // Get user group IDs for the user
        var memberships = await _memberRepository.GetAllAsync();
        var userGroupIds = memberships.Where(m => m.UserId == request.UserId).Select(m => m.UserGroupId).ToList();

        // Get yield summaries for those user groups
        var summaries = await _yieldSummaryRepository.GetAllAsync();
        var filtered = summaries.Where(ys => userGroupIds.Contains(ys.UserGroupId));

        if (request.StartDate.HasValue)
        {
            filtered = filtered.Where(ys => ys.Date >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            filtered = filtered.Where(ys => ys.Date <= request.EndDate.Value);
        }

        var userGroups = await _userGroupRepository.GetAllAsync();
        var userGroupDict = userGroups.ToDictionary(ug => ug.Id, ug => ug.Name);

        return filtered.Select(ys => new YieldSummaryDto
        {
            Id = ys.Id,
            UserId = ys.UserId,
            UserGroupId = ys.UserGroupId,
            UserGroupName = userGroupDict.TryGetValue(ys.UserGroupId, out var name) ? name : string.Empty,
            Date = ys.Date,
            MetricTotalsJson = ys.MetricTotalsJson,
            TotalTaskTime = ys.TotalTaskTime,
            CompletedTasks = ys.CompletedTasks,
            CreatedAt = ys.CreatedAt
        });
    }
}