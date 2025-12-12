using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetMetricTypeQueryHandler : IRequestHandler<GetMetricTypeQuery, MetricTypeDto>
{
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetMetricTypeQueryHandler(
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
    }

    public async Task<MetricTypeDto> Handle(GetMetricTypeQuery request, CancellationToken cancellationToken)
    {
        var metricType = await _metricTypeRepository.GetByIdAsync(request.Id);
        if (metricType == null)
        {
            throw new KeyNotFoundException("MetricType not found");
        }

        // Check if user has access to this metric type's user group
        var memberships = await _memberRepository.GetAllAsync();
        var hasAccess = memberships.Any(m => m.UserId == request.UserId && m.UserGroupId == metricType.UserGroupId);
        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User does not have access to this metric type");
        }

        return new MetricTypeDto
        {
            Id = metricType.Id,
            Key = metricType.Key,
            DisplayName = metricType.DisplayName,
            Type = metricType.Type,
            Unit = metricType.Unit,
            UserGroupId = metricType.UserGroupId,
            CreatedAt = metricType.CreatedAt
        };
    }
}