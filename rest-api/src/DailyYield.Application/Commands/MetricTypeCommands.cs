using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Commands;

public class CreateMetricTypeCommand : IRequest<Guid>
{
    public string Key { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public MetricDataType Type { get; set; } = MetricDataType.Numeric;
    public string? Unit { get; set; }
    public Guid UserGroupId { get; set; }
}

public class UpdateMetricTypeCommand : IRequest
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public MetricDataType Type { get; set; } = MetricDataType.Numeric;
    public string? Unit { get; set; }
}

public class DeleteMetricTypeCommand : IRequest
{
    public Guid Id { get; set; }
}