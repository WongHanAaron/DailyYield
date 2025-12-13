using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetMetricEntryQueryHandler : IRequestHandler<GetMetricEntryQuery, MetricEntryDto>
{
    private readonly IRepository<MetricEntry> _metricEntryRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;

    public GetMetricEntryQueryHandler(
        IRepository<MetricEntry> metricEntryRepository,
        IRepository<MetricType> metricTypeRepository)
    {
        _metricEntryRepository = metricEntryRepository;
        _metricTypeRepository = metricTypeRepository;
    }

    public async Task<MetricEntryDto> Handle(GetMetricEntryQuery request, CancellationToken cancellationToken)
    {
        var entry = await _metricEntryRepository.GetByIdAsync(request.Id);
        if (entry == null)
        {
            throw new KeyNotFoundException("MetricEntry not found");
        }

        if (entry.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not own this metric entry");
        }

        var metricType = await _metricTypeRepository.GetByIdAsync(entry.MetricTypeId);
        var metricTypeKey = metricType?.Key ?? string.Empty;
        var metricTypeDisplayName = metricType?.DisplayName ?? string.Empty;

        return new MetricEntryDto
        {
            Id = entry.Id,
            UserId = entry.UserId,
            MetricTypeId = entry.MetricTypeId,
            MetricTypeKey = metricTypeKey,
            MetricTypeDisplayName = metricTypeDisplayName,
            Type = entry.Type,
            NumericValue = entry.NumericValue,
            BooleanValue = entry.BooleanValue,
            CategoryValue = entry.CategoryValue,
            StartedAt = entry.StartedAt,
            EndedAt = entry.EndedAt,
            Timestamp = entry.Timestamp,
            Metadata = entry.Metadata,
            CreatedAt = entry.CreatedAt
        };
    }
}