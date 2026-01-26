using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Infrastructure.Repositories;

internal sealed class FinancialOperationRepository : Repository<FinancialOperation>, IFinancialOperationRepository
{
    public FinancialOperationRepository(FintrackContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<FinancialOperation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(e => e.TransactionType).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FinancialOperation>> GetAllByUserAndDateRangeAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(e => e.TransactionType)
            .Where(e => e.UserId == userId
            && DateOnly.FromDateTime(e.Date) >= from
            && DateOnly.FromDateTime(e.Date) <= to).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FinancialOperation>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(e => e.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<HashSet<Guid>> GetAllIdsByUserAndDateRangeAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var ids = await _dbSet
            .Include(e => e.TransactionType)
            .Where(e => e.UserId == userId
            && DateOnly.FromDateTime(e.Date) >= from
            && DateOnly.FromDateTime(e.Date) <= to)
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        return ids.ToHashSet();
    }

    public override async Task<FinancialOperation> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return (await _dbSet.AsNoTracking().Include(e => e.TransactionType).Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken))!;
    }

    public async Task<FinancialOperation> GetByIdWithoutTransactionType(Guid id, CancellationToken cancellationToken = default)
    {
        return (await _dbSet.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken))!;
    }
}
