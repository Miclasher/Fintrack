using System.Linq.Expressions;

namespace Fintrack.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity, CancellationToken cancellationToken = default);
    void Remove(T entity, CancellationToken cancellationToken = default);
    void RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
