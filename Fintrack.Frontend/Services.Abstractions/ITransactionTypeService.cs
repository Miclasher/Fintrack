using Fintrack.Shared;

namespace Fintrack.Frontend.Services.Abstractions;

public interface ITransactionTypeService
{
    Task<IEnumerable<TransactionTypeDTO>> GetAllAsync();
    Task<TransactionTypeDTO> GetByIdAsync(Guid id);
    Task CreateAsync(TransactionTypeForCreateDTO transactionType);
    Task UpdateAsync(TransactionTypeDTO transactionType);
    Task DeleteAsync(Guid id);
}
