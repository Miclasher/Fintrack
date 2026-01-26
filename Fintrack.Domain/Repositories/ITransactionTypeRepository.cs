using Fintrack.Domain.Entities;

namespace Fintrack.Domain.Repositories;

public interface ITransactionTypeRepository : IRepository<TransactionType>
{
    Task<IEnumerable<TransactionType>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Dictionary<int, Guid>> GetMccToTransactionTypeIdDictAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, TransactionType>> GetDictByIdListAsync(Guid userId, IEnumerable<Guid> transactionTypeIds, CancellationToken cancellationToken = default);
}
