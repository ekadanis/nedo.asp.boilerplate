using Mapster;
using Microsoft.AspNetCore.Mvc;
using Nedo.Asp.Boilerplate.API.Models.Common;
using Nedo.Asp.Boilerplate.API.Models.Project;
using Nedo.Asp.Boilerplate.Application.DTOs.Project;
using Nedo.Asp.Boilerplate.Application.Interfaces.Services;

namespace Nedo.Asp.Boilerplate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<ProjectListDto>>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var projects = await _projectService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<List<ProjectListDto>>.SuccessResponse(projects, "Projects retrieved successfully"));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var project = await _projectService.GetByIdAsync(id, cancellationToken);
        
        if (project == null)
            return NotFound(ApiResponse<object>.ErrorResponse($"Project with ID {id} not found"));

        return Ok(ApiResponse<ProjectDto>.SuccessResponse(project, "Project retrieved successfully"));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var dto = request.Adapt<ProjectDto>();
        var projectId = await _projectService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = projectId }, 
            ApiResponse<Guid>.SuccessResponse(projectId, "Project created successfully"));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var dto = request.Adapt<ProjectDto>();
        await _projectService.UpdateAsync(id, dto, cancellationToken);

        return Ok(ApiResponse<object>.SuccessResponse(null, "Project updated successfully"));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] string? deletedBy = null, CancellationToken cancellationToken = default)
    {
        await _projectService.DeleteAsync(id, deletedBy ?? "system", cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Project deleted successfully"));
    }
}
