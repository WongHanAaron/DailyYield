using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetYieldSummaryQueryHandler : IRequestHandler<GetYieldSummaryQuery, YieldSummaryDto>
{
    private readonly IRepository<YieldSummary> _yieldSummaryRepository;

    public GetYieldSummaryQueryHandler(IRepository<YieldSummary> yieldSummaryRepository)
    {
        _yieldSummaryRepository = yieldSummaryRepository;
    }

    public async Task<YieldSummaryDto> Handle(GetYieldSummaryQuery request, CancellationToken cancellationToken)
    {
        var summary = await _yieldSummaryRepository.GetByIdAsync(request.Id);
        if (summary == null)
        {
            throw new KeyNotFoundException("YieldSummary not found");
        }

        // Check if user owns this summary
        if (summary.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not have access to this yield summary");
        }

        return new YieldSummaryDto
        {
            Id = summary.Id,
            UserId = summary.UserId,
            Date = summary.Date,
            SummaryData = summary.SummaryData,
            CreatedAt = summary.CreatedAt,
            UpdatedAt = summary.UpdatedAt
        };
    }
}