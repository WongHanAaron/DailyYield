using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class DeleteMetricEntryCommandHandler : IRequestHandler<DeleteMetricEntryCommand>
{
    private readonly IRepository<MetricEntry> _metricEntryRepository;

    public DeleteMetricEntryCommandHandler(IRepository<MetricEntry> metricEntryRepository)
    {
        _metricEntryRepository = metricEntryRepository;
    }

    public async System.Threading.Tasks.Task Handle(DeleteMetricEntryCommand request, CancellationToken cancellationToken)
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

        await _metricEntryRepository.DeleteAsync(metricEntry);
    }
}