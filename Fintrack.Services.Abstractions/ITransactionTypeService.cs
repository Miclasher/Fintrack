using Fintrack.Shared;

namespace Fintrack.Services.Abstractions;

public interface ITransactionTypeService
{
    Task<TransactionTypeDTO> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TransactionTypeDTO>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, Guid targetId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid userId, TransactionTypeDTO transType, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, TransactionTypeForCreateDTO transType, CancellationToken cancellationToken = default);
}
