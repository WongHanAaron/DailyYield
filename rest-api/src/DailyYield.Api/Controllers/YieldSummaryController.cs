using DailyYield.Api.Controllers;
using DailyYield.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyYield.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class YieldSummaryController : BaseController
{
    private readonly IMediator _mediator;

    public YieldSummaryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetYieldSummaries(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var query = new GetYieldSummariesQuery
        {
            UserId = GetCurrentUserId(),
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetYieldSummary(Guid id)
    {
        var query = new GetYieldSummaryQuery
        {
            Id = id,
            UserId = GetCurrentUserId()
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}