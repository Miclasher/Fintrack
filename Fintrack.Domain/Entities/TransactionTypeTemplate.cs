namespace Fintrack.Domain.Entities;

public sealed class TransactionTypeTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsExpense { get; set; }
    public IEnumerable<Mcc> Mccs { get; set; } = new List<Mcc>();
}