namespace Fintrack.Domain.Entities;

public sealed class FinancialOperation
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? UserComment { get; set; }
    public Guid TransactionTypeId { get; set; }
    public TransactionType TransactionType { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
