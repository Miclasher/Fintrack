namespace Fintrack.Domain.Entities;

public sealed class TransactionType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsExpense { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public IEnumerable<Mcc> Mccs { get; set; } = new List<Mcc>();
    public IEnumerable<FinancialOperation> FinancialOperations { get; set; } = new List<FinancialOperation>();
}
