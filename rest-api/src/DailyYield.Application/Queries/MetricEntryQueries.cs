using MediatR;

namespace DailyYield.Application.Queries;

public class GetMetricEntriesQuery : IRequest<IEnumerable<MetricEntryDto>>
{
    public Guid UserId { get; set; }
    public Guid? MetricTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetMetricEntryQuery : IRequest<MetricEntryDto>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class MetricEntryDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MetricTypeId { get; set; }
    public string MetricTypeKey { get; set; } = string.Empty;
    public string MetricTypeDisplayName { get; set; } = string.Empty;
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Notes { get; set; }
}