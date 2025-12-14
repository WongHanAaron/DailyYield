using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetMetricEntriesQueryHandler : IRequestHandler<GetMetricEntriesQuery, IEnumerable<MetricEntryDto>>
{
    private readonly IRepository<MetricEntry> _metricEntryRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IMapper _mapper;

    public GetMetricEntriesQueryHandler(
        IRepository<MetricEntry> metricEntryRepository,
        IRepository<MetricType> metricTypeRepository,
        IMapper mapper)
    {
        _metricEntryRepository = metricEntryRepository;
        _metricTypeRepository = metricTypeRepository;
        _mapper = mapper;
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

        var entryDtos = _mapper.Map<IEnumerable<MetricEntryDto>>(filtered).ToList();
        
        foreach (var entryDto in entryDtos)
        {
            if (metricTypeDict.TryGetValue(entryDto.MetricTypeId, out var mt))
            {
                entryDto.MetricTypeKey = mt.Key;
                entryDto.MetricTypeDisplayName = mt.DisplayName;
            }
        }

        return entryDtos;
    }
}