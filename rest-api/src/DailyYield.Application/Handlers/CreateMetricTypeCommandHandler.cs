using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class CreateMetricTypeCommandHandler : IRequestHandler<CreateMetricTypeCommand, Guid>
{
    private readonly IRepository<MetricType> _metricTypeRepository;

    public CreateMetricTypeCommandHandler(IRepository<MetricType> metricTypeRepository)
    {
        _metricTypeRepository = metricTypeRepository;
    }

    public async Task<Guid> Handle(CreateMetricTypeCommand request, CancellationToken cancellationToken)
    {
        var metricType = new MetricType
        {
            Key = request.Key,
            DisplayName = request.DisplayName,
            Type = request.Type,
            Unit = request.Unit,
            UserGroupId = request.UserGroupId
        };

        await _metricTypeRepository.AddAsync(metricType);
        return metricType.Id;
    }
}