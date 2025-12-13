using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class CreateMetricEntryCommandHandler : IRequestHandler<CreateMetricEntryCommand, Guid>
{
    private readonly IRepository<MetricEntry> _metricEntryRepository;

    public CreateMetricEntryCommandHandler(IRepository<MetricEntry> metricEntryRepository)
    {
        _metricEntryRepository = metricEntryRepository;
    }

    public async Task<Guid> Handle(CreateMetricEntryCommand request, CancellationToken cancellationToken)
    {
        var metricEntry = new MetricEntry
        {
            UserId = request.UserId,
            MetricTypeId = request.MetricTypeId,
            Type = request.Type,
            NumericValue = request.NumericValue,
            BooleanValue = request.BooleanValue,
            CategoryValue = request.CategoryValue,
            StartedAt = request.StartedAt,
            EndedAt = request.EndedAt,
            Timestamp = request.Timestamp ?? DateTime.UtcNow,
            Metadata = request.Metadata
        };

        await _metricEntryRepository.AddAsync(metricEntry);
        return metricEntry.Id;
    }
}