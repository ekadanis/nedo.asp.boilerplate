using System.ComponentModel.DataAnnotations;

namespace Nedo.Asp.Boilerplate.API.Models.Document;

public class UpdateDocumentRequest
{
    [Required(ErrorMessage = "Document title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "File name is required")]
    [MaxLength(255, ErrorMessage = "File name cannot exceed 255 characters")]
    public string FileName { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "File path cannot exceed 500 characters")]
    public string? FilePath { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "File size must be a positive number")]
    public long FileSize { get; set; }

    [MaxLength(100, ErrorMessage = "MIME type cannot exceed 100 characters")]
    public string? MimeType { get; set; }

    public string? UpdatedBy { get; set; }
}
