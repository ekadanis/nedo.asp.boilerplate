using Nedo.AspNet.Request.Validation.Attributes.Generic;
using Nedo.AspNet.Request.Validation.Attributes.String;

namespace Nedo.Asp.Boilerplate.Application.DTOs;

/// <summary>
/// DTO returned in API responses (read model).
/// </summary>
public class StudentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

/// <summary>
/// DTO for creating or updating a student — with Nedo validation attributes.
/// Used inside InsertRequest&lt;CreateStudentDto&gt; and UpdateRequest&lt;CreateStudentDto&gt;.
/// </summary>
public class CreateStudentDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public DateTime EnrollmentDate { get; set; }
}
