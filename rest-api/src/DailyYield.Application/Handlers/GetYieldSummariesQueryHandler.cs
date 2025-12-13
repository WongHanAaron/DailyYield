using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetYieldSummariesQueryHandler : IRequestHandler<GetYieldSummariesQuery, IEnumerable<YieldSummaryDto>>
{
    private readonly IRepository<YieldSummary> _yieldSummaryRepository;

    public GetYieldSummariesQueryHandler(IRepository<YieldSummary> yieldSummaryRepository)
    {
        _yieldSummaryRepository = yieldSummaryRepository;
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

        return userSummaries.Select(ys => new YieldSummaryDto
        {
            Id = ys.Id,
            UserId = ys.UserId,
            Date = ys.Date,
            SummaryData = ys.SummaryData,
            CreatedAt = ys.CreatedAt,
            UpdatedAt = ys.UpdatedAt
        });
    }
}