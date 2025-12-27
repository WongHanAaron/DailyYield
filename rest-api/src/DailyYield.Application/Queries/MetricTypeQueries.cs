using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Queries;

public class GetMetricTypesQuery : IRequest<IEnumerable<MetricTypeDto>>
{
    public Guid UserId { get; set; }
}

public class GetMetricTypeQuery : IRequest<MetricTypeDto>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class MetricTypeDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public MetricDataType Type { get; set; } = MetricDataType.Numeric;
    public string? Unit { get; set; }
    public Guid UserGroupId { get; set; }
    public DateTime CreatedAt { get; set; }
}