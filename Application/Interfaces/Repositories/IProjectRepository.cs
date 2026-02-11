using Nedo.Asp.Boilerplate.Domain.Entities;

namespace Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Project?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<Project?> GetWithDocumentsAsync(Guid id, CancellationToken cancellationToken = default);
}
