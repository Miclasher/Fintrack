namespace Fintrack.Domain.Entities;

public sealed class Mcc
{
    public Guid Id { get; set; }
    public int Value { get; set; }

    public IEnumerable<TransactionType> TransactionTypes { get; set; } = new List<TransactionType>();
    public IEnumerable<TransactionTypeTemplate> TransactionTypeTemplates { get; set; } = new List<TransactionTypeTemplate>();
}
