using Mapster;
using Nedo.Asp.Boilerplate.Application.DTOs.Project;
using Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;
using Nedo.Asp.Boilerplate.Application.Interfaces.Services;
using Nedo.Asp.Boilerplate.Domain.Entities;

namespace Nedo.Asp.Boilerplate.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetWithDocumentsAsync(id, cancellationToken);
        if (project == null) return null;

        var dto = project.Adapt<ProjectDto>();
        dto.DocumentCount = project.Documents?.Count ?? 0;
        return dto;
    }

    public async Task<List<ProjectListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetsAsync(cancellationToken);
        return projects.Adapt<List<ProjectListDto>>();
    }

    public async Task<Guid> CreateAsync(ProjectDto dto, CancellationToken cancellationToken = default)
    {
        var project = dto.Adapt<Project>();
        project.Id = Guid.CreateVersion7();
        project.CreatedDate = DateTimeOffset.UtcNow;
        project.IsActive = true;

        return await _projectRepository.CreateAsync(project, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, ProjectDto dto, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(id, cancellationToken);
        if (project == null)
            throw new KeyNotFoundException($"Project with ID {id} not found");

        // Update properties
        project.Name = dto.Name;
        project.Code = dto.Code;
        project.Description = dto.Description;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.Status = dto.Status;
        project.UpdatedDate = DateTimeOffset.UtcNow;
        project.UpdatedBy = dto.UpdatedBy;

        await _projectRepository.UpdateAsync(project, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, string deletedBy, CancellationToken cancellationToken = default)
    {
        await _projectRepository.DeleteAsync(id, deletedBy, cancellationToken);
    }
}
