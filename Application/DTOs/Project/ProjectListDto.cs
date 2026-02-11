namespace Nedo.Asp.Boilerplate.Application.DTOs.Project;

/// <summary>
/// Lightweight project DTO for list/pagination views
/// </summary>
public class ProjectListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int DocumentCount { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
