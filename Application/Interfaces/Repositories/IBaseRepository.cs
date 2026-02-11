using Nedo.Asp.Boilerplate.Domain.Entities;

namespace Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class, IBaseEntity
{
    Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<T>> GetsAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, string deletedBy, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken = default);
}
