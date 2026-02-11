using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nedo.Asp.Boilerplate.Domain.Entities;

[Table("dbs000_project")]
public class Project : IBaseEntity
{
    // Primary Key
    [Key]
    [Column("project_id")]
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Column("project_name")]
    [MaxLength(200)]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("project_code")]
    [MaxLength(50)]
    [Required]
    public string Code { get; set; } = string.Empty;

    [Column("project_description")]
    public string? Description { get; set; }

    [Column("project_startdate")]
    public DateOnly? StartDate { get; set; }

    [Column("project_enddate")]
    public DateOnly? EndDate { get; set; }

    [Column("project_status")]
    [MaxLength(50)]
    public string Status { get; set; } = "draft";

    [Column("project_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("project_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("project_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("project_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("project_createdby")]
    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    [Column("project_updatedby")]
    [MaxLength(100)]
    public string? UpdatedBy { get; set; }

    [Column("project_deletedby")]
    [MaxLength(100)]
    public string? DeletedBy { get; set; }

    // Navigation Properties
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}
