using MediatR;

namespace DailyYield.Application.Queries;

public class GetYieldSummariesQuery : IRequest<IEnumerable<YieldSummaryDto>>
{
    public Guid UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetYieldSummaryQuery : IRequest<YieldSummaryDto>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class YieldSummaryDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public string SummaryData { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}