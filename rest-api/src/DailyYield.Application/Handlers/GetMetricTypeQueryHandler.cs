using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetMetricTypeQueryHandler : IRequestHandler<GetMetricTypeQuery, MetricTypeDto>
{
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;
    private readonly IMapper _mapper;

    public GetMetricTypeQueryHandler(
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository,
        IMapper mapper)
    {
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
        _mapper = mapper;
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

        return _mapper.Map<MetricTypeDto>(metricType);
    }
}