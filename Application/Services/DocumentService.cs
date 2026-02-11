using Mapster;
using Nedo.Asp.Boilerplate.Application.DTOs.Document;
using Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;
using Nedo.Asp.Boilerplate.Application.Interfaces.Services;
using Nedo.Asp.Boilerplate.Domain.Entities;

namespace Nedo.Asp.Boilerplate.Application.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IProjectRepository _projectRepository;

    public DocumentService(IDocumentRepository documentRepository, IProjectRepository projectRepository)
    {
        _documentRepository = documentRepository;
        _projectRepository = projectRepository;
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetAsync(id, cancellationToken);
        if (document == null) return null;

        var dto = document.Adapt<DocumentDto>();
        
        // Get project name if available
        if (document.Project != null)
        {
            dto.ProjectName = document.Project.Name;
        }

        return dto;
    }

    public async Task<List<DocumentListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var documents = await _documentRepository.GetsAsync(cancellationToken);
        return documents.Adapt<List<DocumentListDto>>();
    }

    public async Task<List<DocumentListDto>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var documents = await _documentRepository.GetByProjectIdAsync(projectId, cancellationToken);
        return documents.Adapt<List<DocumentListDto>>();
    }

    public async Task<Guid> CreateAsync(DocumentDto dto, CancellationToken cancellationToken = default)
    {
        // Validate project exists
        var projectExists = await _projectRepository.IsExistAsync(dto.ProjectId, cancellationToken);
        if (!projectExists)
            throw new KeyNotFoundException($"Project with ID {dto.ProjectId} not found");

        var document = dto.Adapt<Document>();
        document.Id = Guid.CreateVersion7();
        document.CreatedDate = DateTimeOffset.UtcNow;
        document.IsActive = true;

        return await _documentRepository.CreateAsync(document, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, DocumentDto dto, CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetAsync(id, cancellationToken);
        if (document == null)
            throw new KeyNotFoundException($"Document with ID {id} not found");

        // Update properties
        document.Title = dto.Title;
        document.FileName = dto.FileName;
        document.FilePath = dto.FilePath;
        document.FileSize = dto.FileSize;
        document.MimeType = dto.MimeType;
        document.UpdatedDate = DateTimeOffset.UtcNow;
        document.UpdatedBy = dto.UpdatedBy;

        await _documentRepository.UpdateAsync(document, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, string deletedBy, CancellationToken cancellationToken = default)
    {
        await _documentRepository.DeleteAsync(id, deletedBy, cancellationToken);
    }
}
