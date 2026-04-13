using Fintrack.Shared;

namespace Fintrack.Services.Abstractions;

public interface ISummaryExcelExportService
{
    byte[] GenerateExcel(SummaryDTO summary, IEnumerable<TransactionTypeDTO> transactionTypes);
}
