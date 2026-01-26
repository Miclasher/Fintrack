using Fintrack.Frontend.Services.Abstractions;
using Fintrack.Shared;

namespace Fintrack.Frontend.Services;

internal sealed class SummaryService : BaseService, ISummaryService
{
    public SummaryService(IHttpClientFactory httpClientFactory, IAuthService authService) : base(httpClientFactory, authService)
    {
    }

    public async Task<SummaryDTO> GetDateRangeSummary(DateRangeDTO dateRange)
    {
        var summary = await SendAsync<SummaryDTO, DateRangeDTO>("api/Summary/dateRangeSummary/", HttpMethod.Post, dateRange);

        return summary ?? throw new InvalidDataException("Failed to get summary from server response.");
    }

    public async Task<SummaryDTO> GetDaySummary(DateOnly dateOnly)
    {
        var summary = await SendAsync<SummaryDTO, DateOnly>("api/Summary/daySummary/", HttpMethod.Post, dateOnly);

        return summary ?? throw new InvalidDataException("Failed to get summary from server response.");
    }
}
