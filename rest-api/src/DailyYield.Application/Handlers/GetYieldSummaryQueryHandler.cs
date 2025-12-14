using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetYieldSummaryQueryHandler : IRequestHandler<GetYieldSummaryQuery, YieldSummaryDto>
{
    private readonly IRepository<YieldSummary> _yieldSummaryRepository;
    private readonly IMapper _mapper;

    public GetYieldSummaryQueryHandler(IRepository<YieldSummary> yieldSummaryRepository, IMapper mapper)
    {
        _yieldSummaryRepository = yieldSummaryRepository;
        _mapper = mapper;
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

        return _mapper.Map<YieldSummaryDto>(summary);
    }
}