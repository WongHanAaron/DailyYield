using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class DeleteMetricTypeCommandHandler : IRequestHandler<DeleteMetricTypeCommand>
{
    private readonly IRepository<MetricType> _metricTypeRepository;

    public DeleteMetricTypeCommandHandler(IRepository<MetricType> metricTypeRepository)
    {
        _metricTypeRepository = metricTypeRepository;
    }

    public async System.Threading.Tasks.Task Handle(DeleteMetricTypeCommand request, CancellationToken cancellationToken)
    {
        var metricType = await _metricTypeRepository.GetByIdAsync(request.Id);
        if (metricType == null)
        {
            throw new KeyNotFoundException("MetricType not found");
        }

        await _metricTypeRepository.DeleteAsync(metricType);
    }
}