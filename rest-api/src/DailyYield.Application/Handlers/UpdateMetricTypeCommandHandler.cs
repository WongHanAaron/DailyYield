using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class UpdateMetricTypeCommandHandler : IRequestHandler<UpdateMetricTypeCommand>
{
    private readonly IRepository<MetricType> _metricTypeRepository;

    public UpdateMetricTypeCommandHandler(IRepository<MetricType> metricTypeRepository)
    {
        _metricTypeRepository = metricTypeRepository;
    }

    public async System.Threading.Tasks.Task Handle(UpdateMetricTypeCommand request, CancellationToken cancellationToken)
    {
        var metricType = await _metricTypeRepository.GetByIdAsync(request.Id);
        if (metricType == null)
        {
            throw new KeyNotFoundException("MetricType not found");
        }

        metricType.Key = request.Key;
        metricType.DisplayName = request.DisplayName;
        metricType.Type = request.Type;
        metricType.Unit = request.Unit;

        await _metricTypeRepository.UpdateAsync(metricType);
    }
}