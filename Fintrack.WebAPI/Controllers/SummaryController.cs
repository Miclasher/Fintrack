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
    private readonly ITransactionTypeService _transactionTypeService;
    private readonly ISummaryExcelExportService _summaryExcelExportService;

    public SummaryController(ISummaryService summaryService, ITransactionTypeService transactionTypeService, ISummaryExcelExportService summaryExcelExportService)
    {
        _summaryService = summaryService;
        _transactionTypeService = transactionTypeService;
        _summaryExcelExportService = summaryExcelExportService;
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

    [HttpPost("daySummary/exportExcel")]
    public async Task<IActionResult> ExportDaySummaryToExcel([FromBody] DateOnly date)
    {
        var userId = User.GetUserIdFromJwt();

        var summary = await _summaryService.GetDaySummaryAsync(userId, date);
        var transactionTypes = await _transactionTypeService.GetAllAsync(userId);
        var fileBytes = _summaryExcelExportService.GenerateExcel(summary, transactionTypes);

        return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"day-summary-{date:yyyy-MM-dd}.xlsx");
    }

    [HttpPost("dateRangeSummary/exportExcel")]
    public async Task<IActionResult> ExportDateRangeSummaryToExcel([FromBody] DateRangeDTO dateRange)
    {
        var userId = User.GetUserIdFromJwt();

        var summary = await _summaryService.GetDateRangeSummaryAsync(userId, dateRange);
        var transactionTypes = await _transactionTypeService.GetAllAsync(userId);
        var fileBytes = _summaryExcelExportService.GenerateExcel(summary, transactionTypes);

        return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"date-range-summary-{dateRange.StartDate:yyyy-MM-dd}-to-{dateRange.EndDate:yyyy-MM-dd}.xlsx");
    }
}
