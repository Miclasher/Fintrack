using Fintrack.Shared;

namespace Fintrack.Services.Abstractions;

public interface IFinancialOperationService
{
    Task<FinancialOperationDTO> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancialOperationDTO>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, FinancialOperationForCreateDTO finOp, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid userId, FinancialOperationDTO finOp, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, Guid targetId, CancellationToken cancellationToken = default);
}
