using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Infrastructure.Repositories;

internal sealed class TransactionTypeRepository : Repository<TransactionType>, ITransactionTypeRepository
{
    public TransactionTypeRepository(FintrackContext context) : base(context)
    {
    }

    public async override Task<TransactionType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return (await _dbSet.Include(e => e.FinancialOperations).Include(e => e.Mccs).FirstOrDefaultAsync(e => e.Id == id, cancellationToken))!;
    }

    public async Task<IEnumerable<TransactionType>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(e => e.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<int, Guid>> GetMccToTransactionTypeIdDictAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(e => e.UserId == userId)
            .SelectMany(tt => tt.Mccs.Select(mccs => new { mccs.Value, tt.Id }))
            .ToDictionaryAsync(e => e.Value, e => e.Id, cancellationToken);
    }

    public async Task<Dictionary<Guid, TransactionType>> GetDictByIdListAsync(Guid userId, IEnumerable<Guid> transactionTypeIds, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(e => e.UserId == userId)
            .Where(e => transactionTypeIds.Contains(e.Id))
            .ToDictionaryAsync(e => e.Id, cancellationToken);
    }
}
