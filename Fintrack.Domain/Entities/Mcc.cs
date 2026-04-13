using Fintrack.Domain.Entities.Abstract;

namespace Fintrack.Domain.Entities;

public sealed class Mcc : Entity
{
    public int Value { get; set; }

    public IEnumerable<TransactionType> TransactionTypes { get; set; } = new List<TransactionType>();
    public IEnumerable<TransactionTypeTemplate> TransactionTypeTemplates { get; set; } = new List<TransactionTypeTemplate>();
}
