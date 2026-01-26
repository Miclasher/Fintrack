using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Infrastructure.Repositories;

internal class TransactionTypeRepositoryTemplate : Repository<TransactionTypeTemplate>, ITransactionTypeTemplateRepository
{
    public TransactionTypeRepositoryTemplate(FintrackContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TransactionTypeTemplate>> GetAllWithMccAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(e => e.Mccs).ToListAsync(cancellationToken);
    }
}