using System.ComponentModel.DataAnnotations;

namespace Nedo.Asp.Boilerplate.API.Models.Project;

public class CreateProjectRequest
{
    [Required(ErrorMessage = "Project name is required")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Project code is required")]
    [MaxLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
    public string Code { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "draft";

    public string? CreatedBy { get; set; }
}
