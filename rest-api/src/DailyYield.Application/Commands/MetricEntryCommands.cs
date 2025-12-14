using MediatR;
using DailyYield.Domain.Entities;

namespace DailyYield.Application.Commands;

public class CreateMetricEntryCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid MetricTypeId { get; set; }
    public MetricEntryType Type { get; set; }
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Metadata { get; set; }
}

public class UpdateMetricEntryCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public MetricEntryType Type { get; set; }
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Metadata { get; set; }
}

public class DeleteMetricEntryCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}