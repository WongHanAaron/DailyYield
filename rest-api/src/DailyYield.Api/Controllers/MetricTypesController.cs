using DailyYield.Api.Controllers;
using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace DailyYield.Api.Controllers;

/// <summary>
/// Controller for managing metric types
/// </summary>
[Route("api/metric-types")]
public class MetricTypesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MetricTypesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all metric types for the authenticated user's user groups
    /// </summary>
    /// <returns>List of metric types</returns>
    [HttpGet]
    public async Task<IActionResult> GetMetricTypes()
    {
        var userId = GetCurrentUserId();
        var query = new GetMetricTypesQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific metric type by ID
    /// </summary>
    /// <param name="id">The metric type ID</param>
    /// <returns>The metric type details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMetricType(Guid id)
    {
        var userId = GetCurrentUserId();
        var query = new GetMetricTypeQuery { Id = id, UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new metric type
    /// </summary>
    /// <param name="request">The metric type creation data</param>
    /// <returns>The created metric type ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMetricType([FromBody] CreateMetricTypeRequest request)
    {
        var userId = GetCurrentUserId();
        // Note: In a real implementation, you'd validate that the user has access to the UserGroupId
        var command = _mapper.Map<CreateMetricTypeCommand>(request);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMetricType), new { id = result }, result);
    }

    /// <summary>
    /// Updates an existing metric type
    /// </summary>
    /// <param name="id">The metric type ID</param>
    /// <param name="request">The updated metric type data</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMetricType(Guid id, [FromBody] UpdateMetricTypeRequest request)
    {
        var userId = GetCurrentUserId();
        // Note: Authorization check should be in the handler
        var command = _mapper.Map<UpdateMetricTypeCommand>(request);
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a metric type
    /// </summary>
    /// <param name="id">The metric type ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMetricType(Guid id)
    {
        var userId = GetCurrentUserId();
        // Note: Authorization check should be in the handler
        var command = new DeleteMetricTypeCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}

public class CreateMetricTypeRequest
{
    public string Key { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public MetricDataType Type { get; set; } = MetricDataType.Numeric;
    public string? Unit { get; set; }
    public Guid UserGroupId { get; set; }
}

public class UpdateMetricTypeRequest
{
    public string Key { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public MetricDataType Type { get; set; } = MetricDataType.Numeric;
    public string? Unit { get; set; }
}