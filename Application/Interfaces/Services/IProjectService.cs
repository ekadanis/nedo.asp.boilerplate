using Nedo.Asp.Boilerplate.Application.DTOs.Project;

namespace Nedo.Asp.Boilerplate.Application.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ProjectListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(ProjectDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, ProjectDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, string deletedBy, CancellationToken cancellationToken = default);
}
