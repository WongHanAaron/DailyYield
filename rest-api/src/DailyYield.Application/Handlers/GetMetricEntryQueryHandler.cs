using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetMetricEntryQueryHandler : IRequestHandler<GetMetricEntryQuery, MetricEntryDto>
{
    private readonly IRepository<MetricEntry> _metricEntryRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IMapper _mapper;

    public GetMetricEntryQueryHandler(
        IRepository<MetricEntry> metricEntryRepository,
        IRepository<MetricType> metricTypeRepository,
        IMapper mapper)
    {
        _metricEntryRepository = metricEntryRepository;
        _metricTypeRepository = metricTypeRepository;
        _mapper = mapper;
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

        var entryDto = _mapper.Map<MetricEntryDto>(entry);
        entryDto.MetricTypeKey = metricType?.Key ?? string.Empty;
        entryDto.MetricTypeDisplayName = metricType?.DisplayName ?? string.Empty;

        return entryDto;
    }
}