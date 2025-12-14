using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetYieldSummariesQueryHandler : IRequestHandler<GetYieldSummariesQuery, IEnumerable<YieldSummaryDto>>
{
    private readonly IRepository<YieldSummary> _yieldSummaryRepository;
    private readonly IMapper _mapper;

    public GetYieldSummariesQueryHandler(IRepository<YieldSummary> yieldSummaryRepository, IMapper mapper)
    {
        _yieldSummaryRepository = yieldSummaryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<YieldSummaryDto>> Handle(GetYieldSummariesQuery request, CancellationToken cancellationToken)
    {
        var summaries = await _yieldSummaryRepository.GetAllAsync();
        var userSummaries = summaries.Where(ys => ys.UserId == request.UserId);

        if (request.StartDate.HasValue)
        {
            userSummaries = userSummaries.Where(ys => ys.Date >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            userSummaries = userSummaries.Where(ys => ys.Date <= request.EndDate.Value);
        }

        return _mapper.Map<IEnumerable<YieldSummaryDto>>(userSummaries);
    }
}