namespace Fintrack.Shared;

public sealed class SummaryDTO
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public IEnumerable<FinancialOperationDTO> Operations { get; set; } = new List<FinancialOperationDTO>();
}
