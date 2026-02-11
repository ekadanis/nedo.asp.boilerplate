using Microsoft.EntityFrameworkCore;
using Nedo.Asp.Boilerplate.Domain.Entities;

namespace Nedo.Asp.Boilerplate.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasQueryFilter(p => p.IsActive);

            entity.HasMany(p => p.Documents)
                  .WithOne(d => d.Project)
                  .HasForeignKey(d => d.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(p => p.Code)
                  .IsUnique()
                  .HasDatabaseName("uq_dbs000_project_code");

            entity.HasIndex(p => p.CreatedDate)
                  .HasDatabaseName("ix_dbs000_project_createddate");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasQueryFilter(d => d.IsActive);

            entity.HasIndex(d => d.ProjectId)
                  .HasDatabaseName("ix_dbs000_document_projectid");

            entity.HasIndex(d => d.CreatedDate)
                  .HasDatabaseName("ix_dbs000_document_createddate");
        });
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<IBaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTimeOffset.UtcNow;
                    entry.Entity.IsActive = true;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTimeOffset.UtcNow;
                    break;
            }
        }
    }
}
