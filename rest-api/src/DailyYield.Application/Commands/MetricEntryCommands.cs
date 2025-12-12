using MediatR;

namespace DailyYield.Application.Commands;

public class CreateMetricEntryCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid MetricTypeId { get; set; }
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Notes { get; set; }
}

public class UpdateMetricEntryCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Notes { get; set; }
}

public class DeleteMetricEntryCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}