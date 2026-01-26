using Fintrack.Domain.Repositories;

namespace Fintrack.Infrastructure.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly FintrackContext _context;

    public UnitOfWork(FintrackContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
