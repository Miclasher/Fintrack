using Fintrack.Domain.Entities;

namespace Fintrack.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
}
