using Fintrack.Frontend.Services.Abstractions;
using Fintrack.Shared;

namespace Fintrack.Frontend.Services;

internal sealed class SummaryExportService : BaseService, ISummaryExportService
{
    public SummaryExportService(IHttpClientFactory httpClientFactory, IAuthService authService) : base(httpClientFactory, authService)
    {
    }

    public async Task<byte[]> ExportDaySummaryToExcel(DateOnly dateOnly)
    {
        var response = await SendAsync("api/Summary/daySummary/exportExcel/", HttpMethod.Post, dateOnly);

        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<byte[]> ExportDateRangeSummaryToExcel(DateRangeDTO dateRange)
    {
        var response = await SendAsync("api/Summary/dateRangeSummary/exportExcel/", HttpMethod.Post, dateRange);

        return await response.Content.ReadAsByteArrayAsync();
    }
}
