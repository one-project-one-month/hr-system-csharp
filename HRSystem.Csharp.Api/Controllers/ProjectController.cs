using HRSystem.Csharp.Domain.Features.Project;
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly BL_Project _blProject;

    public ProjectController(BL_Project blProject)
    {
        _blProject = blProject;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetALlProjects([FromQuery] ProjectListRequestModel reqModel)
    {
        var result = await _blProject.GetAllProjects(reqModel);
        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        return StatusCode(500, result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProject([FromBody] ProjectRequestModel project)
    {
        var result = await _blProject.CreateProject(project);

        if (result.IsSuccess) return Ok(result);

        if (result.IsDuplicateRecord) return BadRequest(result);

        if (result.IsInvalidData) return BadRequest(result);

        return StatusCode(500, result);
    }

    [HttpGet("edit/{projectCode}")]
    public async Task<IActionResult> GetProject(string projectCode)
    {
        var result = await _blProject.GetProject(new ProjectEditRequestModel
        {
            ProjectCode = projectCode
        });
        if (result.IsSuccess) return Ok(result);

        if (result.IsNotFound) return NotFound(result);

        return StatusCode(500, result);
    }

    [HttpPut("update/{projectCode}")]
    public async Task<IActionResult> UpdateProject(string projectCode, ProjectRequestModel project)
    {
        var result = await _blProject.UpdateProject(projectCode, project);

        if (result.IsSuccess) return Ok(result);

        if (result.IsNotFound) return NotFound(result);

        if (result.IsValidationError) return BadRequest(result);

        return StatusCode(500, result);
    }

    [HttpDelete("delete/{projectCode}")]
    public async Task<IActionResult> DeleteProject(string projectCode)
    {
        if (string.IsNullOrWhiteSpace(projectCode))
        {
            var error = Result<bool>.ValidationError("Project code is required!");
            return BadRequest(error);
        }

        var result = await _blProject.DeleteProject(projectCode);

        if (result.IsSuccess) return Ok(result);

        if (result.IsNotFound) return NotFound(result);

        return StatusCode(500, result);
    }
}