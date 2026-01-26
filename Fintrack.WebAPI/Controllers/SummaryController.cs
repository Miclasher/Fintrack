using Fintrack.Services.Abstractions;
using Fintrack.Shared;
using Fintrack.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SummaryController : ControllerBase
{
    private readonly ISummaryService _summaryService;

    public SummaryController(ISummaryService summaryService)
    {
        _summaryService = summaryService;
    }

    [HttpPost("daySummary")]
    public async Task<ActionResult<SummaryDTO>> GetDaySummary([FromBody] DateOnly date)
    {
        var userId = User.GetUserIdFromJwt();

        var summary = await _summaryService.GetDaySummaryAsync(userId, date);
        return Ok(summary);
    }

    [HttpPost("dateRangeSummary")]
    public async Task<ActionResult<SummaryDTO>> GetDateRangeSummary([FromBody] DateRangeDTO dateRange)
    {
        var userId = User.GetUserIdFromJwt();

        var summary = await _summaryService.GetDateRangeSummaryAsync(userId, dateRange);
        return Ok(summary);
    }
}
