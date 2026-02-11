using Nedo.Asp.Boilerplate.Domain.Entities;

namespace Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;

public interface IDocumentRepository : IBaseRepository<Document>
{
    Task<List<Document>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}
