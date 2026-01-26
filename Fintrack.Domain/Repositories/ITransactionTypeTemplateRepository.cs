using Fintrack.Domain.Entities;

namespace Fintrack.Domain.Repositories;

public interface ITransactionTypeTemplateRepository : IRepository<TransactionTypeTemplate>
{
    Task<IEnumerable<TransactionTypeTemplate>> GetAllWithMccAsync(CancellationToken cancellationToken = default);
}
