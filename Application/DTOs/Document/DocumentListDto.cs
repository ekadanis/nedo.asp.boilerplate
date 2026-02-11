namespace Nedo.Asp.Boilerplate.Application.DTOs.Document;

public class DocumentListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? MimeType { get; set; }
    public Guid ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
