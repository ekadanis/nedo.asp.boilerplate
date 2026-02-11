using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nedo.Asp.Boilerplate.Domain.Entities;

[Table("dbs000_document")]
public class Document : IBaseEntity
{
    // Primary Key
    [Key]
    [Column("document_id")]
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Column("document_title")]
    [MaxLength(200)]
    [Required]
    public string Title { get; set; } = string.Empty;

    [Column("document_filename")]
    [MaxLength(255)]
    [Required]
    public string FileName { get; set; } = string.Empty;

    [Column("document_filepath")]
    [MaxLength(500)]
    public string? FilePath { get; set; }

    [Column("document_filesize")]
    public long FileSize { get; set; }

    [Column("document_mimetype")]
    [MaxLength(100)]
    public string? MimeType { get; set; }

    // Foreign Key
    [Column("document_projectid")]
    [Required]
    public Guid ProjectId { get; set; }

    // Audit Trail
    [Column("document_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("document_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("document_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("document_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("document_createdby")]
    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    [Column("document_updatedby")]
    [MaxLength(100)]
    public string? UpdatedBy { get; set; }

    [Column("document_deletedby")]
    [MaxLength(100)]
    public string? DeletedBy { get; set; }

    // Navigation Properties
    [ForeignKey(nameof(ProjectId))]
    public Project? Project { get; set; }
}
