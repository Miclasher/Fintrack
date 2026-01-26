namespace Fintrack.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string MonobankAccountId { get; set; } = string.Empty;
    public IEnumerable<FinancialOperation> FinancialOperations { get; set; } = new List<FinancialOperation>();
    public IEnumerable<TransactionType> TransactionTypes { get; set; } = new List<TransactionType>();
}
