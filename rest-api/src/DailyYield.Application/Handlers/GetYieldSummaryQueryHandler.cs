using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetYieldSummaryQueryHandler : IRequestHandler<GetYieldSummaryQuery, YieldSummaryDto>
{
    private readonly IRepository<YieldSummary> _yieldSummaryRepository;
    private readonly IRepository<UserGroup> _userGroupRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetYieldSummaryQueryHandler(
        IRepository<YieldSummary> yieldSummaryRepository,
        IRepository<UserGroup> userGroupRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _yieldSummaryRepository = yieldSummaryRepository;
        _userGroupRepository = userGroupRepository;
        _memberRepository = memberRepository;
    }

    public async Task<YieldSummaryDto> Handle(GetYieldSummaryQuery request, CancellationToken cancellationToken)
    {
        var summary = await _yieldSummaryRepository.GetByIdAsync(request.Id);
        if (summary == null)
        {
            throw new KeyNotFoundException("YieldSummary not found");
        }

        // Check if user has access to this summary's user group
        var memberships = await _memberRepository.GetAllAsync();
        var hasAccess = memberships.Any(m => m.UserId == request.UserId && m.UserGroupId == summary.UserGroupId);
        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User does not have access to this yield summary");
        }

        var userGroup = await _userGroupRepository.GetByIdAsync(summary.UserGroupId);

        return new YieldSummaryDto
        {
            Id = summary.Id,
            UserId = summary.UserId,
            UserGroupId = summary.UserGroupId,
            UserGroupName = userGroup?.Name ?? string.Empty,
            Date = summary.Date,
            MetricTotalsJson = summary.MetricTotalsJson,
            TotalTaskTime = summary.TotalTaskTime,
            CompletedTasks = summary.CompletedTasks,
            CreatedAt = summary.CreatedAt
        };
    }
}