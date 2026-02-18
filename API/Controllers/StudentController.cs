using Microsoft.AspNetCore.Mvc;
using Nedo.AspNet.ApiContracts.Requests;
using Nedo.AspNet.ApiContracts.Responses;
using Nedo.Asp.Boilerplate.Application.DTOs;

namespace Nedo.Asp.Boilerplate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    // In-memory store (no database — DTO layer only)
    private static readonly List<StudentDto> _students = new()
    {
        new StudentDto
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            EnrollmentDate = DateTime.UtcNow
        },
        new StudentDto
        {
            Id = Guid.NewGuid(),
            Name = "Jane Smith",
            Email = "jane.smith@example.com",
            DateOfBirth = new DateTime(2001, 5, 15),
            EnrollmentDate = DateTime.UtcNow
        }
    };

    // ── GET: Query students (paginated) ──────────────────────────────────────

    /// <summary>
    /// Query students with optional filtering, sorting, and pagination.
    /// Uses POST because the query body can be complex.
    /// </summary>
    [HttpPost("query")]
    public ActionResult<BaseResponse<List<StudentDto>>> Query([FromBody] GetRequest request)
    {
        var query = request.Data;
        IEnumerable<StudentDto> result = _students;

        // Apply simple search
        if (!string.IsNullOrWhiteSpace(query?.Search))
        {
            var term = query.Search.ToLowerInvariant();
            result = result.Where(s =>
                s.Name.ToLowerInvariant().Contains(term) ||
                s.Email.ToLowerInvariant().Contains(term));
        }

        // Apply filters
        if (query?.Filters is { Count: > 0 })
        {
            foreach (var filter in query.Filters)
            {
                result = filter switch
                {
                    { Field: "name", Operator: "Contains" } =>
                        result.Where(s => s.Name.Contains(filter.Value?.ToString() ?? "", StringComparison.OrdinalIgnoreCase)),
                    { Field: "email", Operator: "Eq" } =>
                        result.Where(s => s.Email.Equals(filter.Value?.ToString(), StringComparison.OrdinalIgnoreCase)),
                    _ => result
                };
            }
        }

        // Apply sort
        if (query?.Sort is { Count: > 0 })
        {
            var sort = query.Sort[0];
            result = sort.Direction?.ToLowerInvariant() == "desc"
                ? result.OrderByDescending(s => sort.Field == "email" ? (object)s.Email : s.Name)
                : result.OrderBy(s => sort.Field == "email" ? (object)s.Email : s.Name);
        }

        var items = result.ToList();

        // Apply pagination
        var page = query?.Pagination?.Page ?? 1;
        var pageSize = query?.Pagination?.PageSize ?? 10;
        var total = items.Count;
        var paged = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return Ok(BaseResponse<StudentDto>.Paged(paged, page, pageSize, total, "Students retrieved"));
    }

    // ── GET: Single student by ID ─────────────────────────────────────────────

    /// <summary>
    /// Get a single student by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public ActionResult<BaseResponse<StudentDto>> GetById(Guid id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student is null)
            return NotFound(BaseResponse<StudentDto>.Fail("Student not found", 404, ResponseCodes.NotFound));

        return Ok(BaseResponse<StudentDto>.Ok(student, "Student retrieved"));
    }

    // ── POST: Insert student ──────────────────────────────────────────────────

    /// <summary>
    /// Create a new student.
    /// Validation attributes on CreateStudentDto are enforced automatically.
    /// </summary>
    [HttpPost]
    public ActionResult<BaseResponse<StudentDto>> Insert([FromBody] InsertRequest<CreateStudentDto> request)
    {
        if (request.Data is null)
            return BadRequest(BaseResponse<StudentDto>.Fail("Data is required"));

        var student = new StudentDto
        {
            Id = Guid.NewGuid(),
            Name = request.Data.Name,
            Email = request.Data.Email,
            DateOfBirth = request.Data.DateOfBirth,
            EnrollmentDate = request.Data.EnrollmentDate
        };

        _students.Add(student);
        return Ok(BaseResponse<StudentDto>.Ok(student, "Student created"));
    }

    // ── PUT: Update student ───────────────────────────────────────────────────

    /// <summary>
    /// Update an existing student by ID.
    /// </summary>
    [HttpPut]
    public ActionResult<BaseResponse<StudentDto>> Update([FromBody] UpdateRequest<CreateStudentDto> request)
    {
        if (!Guid.TryParse(request.Id, out var id))
            return BadRequest(BaseResponse<StudentDto>.Fail("Invalid Id format"));

        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student is null)
            return NotFound(BaseResponse<StudentDto>.Fail("Student not found", 404, ResponseCodes.NotFound));

        if (request.Data is not null)
        {
            student.Name = request.Data.Name;
            student.Email = request.Data.Email;
            student.DateOfBirth = request.Data.DateOfBirth;
            student.EnrollmentDate = request.Data.EnrollmentDate;
        }

        return Ok(BaseResponse<StudentDto>.Ok(student, "Student updated"));
    }

    // ── DELETE: Delete student ────────────────────────────────────────────────

    /// <summary>
    /// Delete a student by ID. Supports soft delete via soft_delete flag.
    /// </summary>
    [HttpDelete]
    public ActionResult<BaseResponse<StudentDto>> Delete([FromBody] DeleteRequest<string> request)
    {
        if (!Guid.TryParse(request.Data, out var id))
            return BadRequest(BaseResponse<StudentDto>.Fail("Invalid Id format"));

        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student is null)
            return NotFound(BaseResponse<StudentDto>.Fail("Student not found", 404, ResponseCodes.NotFound));

        _students.Remove(student);
        return Ok(BaseResponse<StudentDto>.Ok(student, "Student deleted"));
    }
}
