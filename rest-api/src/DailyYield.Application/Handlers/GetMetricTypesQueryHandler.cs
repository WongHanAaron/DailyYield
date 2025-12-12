using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetMetricTypesQueryHandler : IRequestHandler<GetMetricTypesQuery, IEnumerable<MetricTypeDto>>
{
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetMetricTypesQueryHandler(
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<MetricTypeDto>> Handle(GetMetricTypesQuery request, CancellationToken cancellationToken)
    {
        // Get user group IDs for the user
        var memberships = await _memberRepository.GetAllAsync();
        var userGroupIds = memberships.Where(m => m.UserId == request.UserId).Select(m => m.UserGroupId).ToList();

        // Get metric types for those user groups
        var metricTypes = await _metricTypeRepository.GetAllAsync();
        var filtered = metricTypes.Where(mt => userGroupIds.Contains(mt.UserGroupId));

        return filtered.Select(mt => new MetricTypeDto
        {
            Id = mt.Id,
            Key = mt.Key,
            DisplayName = mt.DisplayName,
            Type = mt.Type,
            Unit = mt.Unit,
            UserGroupId = mt.UserGroupId,
            CreatedAt = mt.CreatedAt
        });
    }
}