using Fintrack.Domain.Entities.Abstract;

namespace Fintrack.Domain.Entities;

public sealed class TransactionTypeTemplate : Entity
{
    public string Name { get; set; } = string.Empty;
    public bool IsExpense { get; set; }
    public IEnumerable<Mcc> Mccs { get; set; } = new List<Mcc>();
}
