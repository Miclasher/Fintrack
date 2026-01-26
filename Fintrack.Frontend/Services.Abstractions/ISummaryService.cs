using Fintrack.Shared;

namespace Fintrack.Frontend.Services.Abstractions;

public interface ISummaryService
{
    Task<SummaryDTO> GetDaySummary(DateOnly dateOnly);
    Task<SummaryDTO> GetDateRangeSummary(DateRangeDTO dateRange);
}
