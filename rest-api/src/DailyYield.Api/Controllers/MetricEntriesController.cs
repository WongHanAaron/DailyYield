using DailyYield.Api.Controllers;
using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace DailyYield.Api.Controllers;

/// <summary>
/// Controller for managing metric entries
/// </summary>
[Route("api/metric-entries")]
public class MetricEntriesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MetricEntriesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all metric entries for the authenticated user
    /// </summary>
    /// <param name="metricTypeId">Optional filter by metric type</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>List of metric entries</returns>
    [HttpGet]
    public async Task<IActionResult> GetMetricEntries(
        [FromQuery] Guid? metricTypeId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var userId = GetCurrentUserId();
        var query = new GetMetricEntriesQuery
        {
            UserId = userId,
            MetricTypeId = metricTypeId,
            StartDate = startDate,
            EndDate = endDate
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific metric entry by ID
    /// </summary>
    /// <param name="id">The metric entry ID</param>
    /// <returns>The metric entry details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMetricEntry(Guid id)
    {
        var userId = GetCurrentUserId();
        var query = new GetMetricEntryQuery { Id = id, UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new metric entry
    /// </summary>
    /// <param name="request">The metric entry creation data</param>
    /// <returns>The created metric entry ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMetricEntry([FromBody] CreateMetricEntryRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<CreateMetricEntryCommand>(request);
        command.UserId = userId;
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMetricEntry), new { id = result }, result);
    }

    /// <summary>
    /// Updates an existing metric entry
    /// </summary>
    /// <param name="id">The metric entry ID</param>
    /// <param name="request">The updated metric entry data</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMetricEntry(Guid id, [FromBody] UpdateMetricEntryRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<UpdateMetricEntryCommand>(request);
        command.Id = id;
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a metric entry
    /// </summary>
    /// <param name="id">The metric entry ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMetricEntry(Guid id)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteMetricEntryCommand { Id = id, UserId = userId };
        await _mediator.Send(command);
        return NoContent();
    }
}

public class CreateMetricEntryRequest
{
    public Guid MetricTypeId { get; set; }
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Notes { get; set; }
}

public class UpdateMetricEntryRequest
{
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Notes { get; set; }
}