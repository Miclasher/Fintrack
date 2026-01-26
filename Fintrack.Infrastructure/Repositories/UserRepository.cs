using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(FintrackContext context) : base(context)
    {
    }

    public override async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return (await _dbSet.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken))!;
    }

    public async Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return (await _dbSet.AsNoTracking().Where(e => e.Username == username).FirstOrDefaultAsync(cancellationToken))!;
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(e => e.Username == username).AnyAsync(cancellationToken);
    }
}
