using Nedo.Asp.Boilerplate.Application.DTOs.Document;

namespace Nedo.Asp.Boilerplate.Application.Interfaces.Services;

public interface IDocumentService
{
    Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DocumentListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<DocumentListDto>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(DocumentDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, DocumentDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, string deletedBy, CancellationToken cancellationToken = default);
}
