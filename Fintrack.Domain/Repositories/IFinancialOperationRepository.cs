using Fintrack.Domain.Entities;

namespace Fintrack.Domain.Repositories;

public interface IFinancialOperationRepository : IRepository<FinancialOperation>
{
    Task<IEnumerable<FinancialOperation>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancialOperation>> GetAllByUserAndDateRangeAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<HashSet<Guid>> GetAllIdsByUserAndDateRangeAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<FinancialOperation> GetByIdWithoutTransactionType(Guid id, CancellationToken cancellationToken = default);
}
