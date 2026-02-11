using Mapster;
using Microsoft.AspNetCore.Mvc;
using Nedo.Asp.Boilerplate.API.Models.Common;
using Nedo.Asp.Boilerplate.API.Models.Document;
using Nedo.Asp.Boilerplate.Application.DTOs.Document;
using Nedo.Asp.Boilerplate.Application.Interfaces.Services;

namespace Nedo.Asp.Boilerplate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentsController> _logger;

    public DocumentsController(IDocumentService documentService, ILogger<DocumentsController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all documents
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<DocumentListDto>>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var documents = await _documentService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<List<DocumentListDto>>.SuccessResponse(documents, "Documents retrieved successfully"));
    }

    /// <summary>
    /// Get document by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<DocumentDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var document = await _documentService.GetByIdAsync(id, cancellationToken);
        
        if (document == null)
            return NotFound(ApiResponse<object>.ErrorResponse($"Document with ID {id} not found"));

        return Ok(ApiResponse<DocumentDto>.SuccessResponse(document, "Document retrieved successfully"));
    }

    /// <summary>
    /// Get documents by project ID
    /// </summary>
    [HttpGet("project/{projectId}")]
    [ProducesResponseType(typeof(ApiResponse<List<DocumentListDto>>), 200)]
    public async Task<IActionResult> GetByProjectId(Guid projectId, CancellationToken cancellationToken)
    {
        var documents = await _documentService.GetByProjectIdAsync(projectId, cancellationToken);
        return Ok(ApiResponse<List<DocumentListDto>>.SuccessResponse(documents, "Documents retrieved successfully"));
    }

    /// <summary>
    /// Create a new document
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateDocumentRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var dto = request.Adapt<DocumentDto>();
        var documentId = await _documentService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = documentId }, 
            ApiResponse<Guid>.SuccessResponse(documentId, "Document created successfully"));
    }

    /// <summary>
    /// Update an existing document
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDocumentRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var dto = request.Adapt<DocumentDto>();
        await _documentService.UpdateAsync(id, dto, cancellationToken);

        return Ok(ApiResponse<object>.SuccessResponse(null, "Document updated successfully"));
    }

    /// <summary>
    /// Delete a document (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] string? deletedBy = null, CancellationToken cancellationToken = default)
    {
        await _documentService.DeleteAsync(id, deletedBy ?? "system", cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Document deleted successfully"));
    }
}
