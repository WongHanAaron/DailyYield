using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetMetricEntriesQueryHandler : IRequestHandler<GetMetricEntriesQuery, IEnumerable<MetricEntryDto>>
{
    private readonly IRepository<MetricEntry> _metricEntryRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;

    public GetMetricEntriesQueryHandler(
        IRepository<MetricEntry> metricEntryRepository,
        IRepository<MetricType> metricTypeRepository)
    {
        _metricEntryRepository = metricEntryRepository;
        _metricTypeRepository = metricTypeRepository;
    }

    public async Task<IEnumerable<MetricEntryDto>> Handle(GetMetricEntriesQuery request, CancellationToken cancellationToken)
    {
        var entries = await _metricEntryRepository.GetAllAsync();
        var filtered = entries.Where(e => e.UserId == request.UserId);

        if (request.MetricTypeId.HasValue)
        {
            filtered = filtered.Where(e => e.MetricTypeId == request.MetricTypeId.Value);
        }

        if (request.StartDate.HasValue)
        {
            filtered = filtered.Where(e => e.Timestamp >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            filtered = filtered.Where(e => e.Timestamp <= request.EndDate.Value);
        }

        var metricTypes = await _metricTypeRepository.GetAllAsync();
        var metricTypeDict = metricTypes.ToDictionary(mt => mt.Id, mt => (mt.Key, mt.DisplayName));

        return filtered.Select(e => new MetricEntryDto
        {
            Id = e.Id,
            UserId = e.UserId,
            MetricTypeId = e.MetricTypeId,
            MetricTypeKey = metricTypeDict.TryGetValue(e.MetricTypeId, out var mt) ? mt.Key : string.Empty,
            MetricTypeDisplayName = metricTypeDict.TryGetValue(e.MetricTypeId, out mt) ? mt.DisplayName : string.Empty,
            Type = e.Type,
            NumericValue = e.NumericValue,
            BooleanValue = e.BooleanValue,
            CategoryValue = e.CategoryValue,
            StartedAt = e.StartedAt,
            EndedAt = e.EndedAt,
            Timestamp = e.Timestamp,
            Metadata = e.Metadata,
            CreatedAt = e.CreatedAt
        });
    }
}