using Fintrack.Shared;

namespace Fintrack.Frontend.Services.Abstractions;

public interface ISummaryExportService
{
    Task<byte[]> ExportDaySummaryToExcel(DateOnly dateOnly);
    Task<byte[]> ExportDateRangeSummaryToExcel(DateRangeDTO dateRange);
}
