using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class UpdateMetricEntryCommandHandler : IRequestHandler<UpdateMetricEntryCommand>
{
    private readonly IRepository<MetricEntry> _metricEntryRepository;

    public UpdateMetricEntryCommandHandler(IRepository<MetricEntry> metricEntryRepository)
    {
        _metricEntryRepository = metricEntryRepository;
    }

    public async System.Threading.Tasks.Task Handle(UpdateMetricEntryCommand request, CancellationToken cancellationToken)
    {
        var metricEntry = await _metricEntryRepository.GetByIdAsync(request.Id);
        if (metricEntry == null)
        {
            throw new KeyNotFoundException("MetricEntry not found");
        }

        if (metricEntry.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not own this metric entry");
        }

        metricEntry.NumericValue = request.NumericValue;
        metricEntry.BooleanValue = request.BooleanValue;
        metricEntry.CategoryValue = request.CategoryValue;
        metricEntry.Timestamp = request.Timestamp ?? metricEntry.Timestamp;
        metricEntry.Notes = request.Notes;

        await _metricEntryRepository.UpdateAsync(metricEntry);
    }
}