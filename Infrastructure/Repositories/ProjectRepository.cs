using Microsoft.EntityFrameworkCore;
using Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;
using Nedo.Asp.Boilerplate.Domain.Entities;
using Nedo.Asp.Boilerplate.Infrastructure.Data;

namespace Nedo.Asp.Boilerplate.Infrastructure.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Project?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public async Task<Project?> GetWithDocumentsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Documents)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
