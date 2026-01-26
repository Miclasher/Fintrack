using Fintrack.Shared;

namespace Fintrack.Frontend.Services.Abstractions;

public interface IMonobankImportService
{
    Task<SummaryDTO> ImportFromMonobankAsync(DateRangeDTO dateRange);
}
