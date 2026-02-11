using Microsoft.EntityFrameworkCore;
using Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;
using Nedo.Asp.Boilerplate.Domain.Entities;
using Nedo.Asp.Boilerplate.Infrastructure.Data;

namespace Nedo.Asp.Boilerplate.Infrastructure.Repositories;

public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
{
    public DocumentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Document>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.ProjectId == projectId)
            .OrderByDescending(d => d.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Document?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Project)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
