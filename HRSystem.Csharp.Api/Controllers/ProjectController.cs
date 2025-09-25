using HRSystem.Csharp.Domain.Features.Project;
using HRSystem.Csharp.Domain.Models.Project;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult GetALlProjects()
        {
                var result = _blProject.GetAllProjects();

                if (!result.IsSuccess) return BadRequest(result);
                return StatusCode(200, result.Data);
        }

        [HttpGet("{code}")]
        public IActionResult GetProject(string code)
        {
                var result = _blProject.GetProject(code);

                if (!result.IsSuccess) return BadRequest(result);
                return StatusCode(200, result.Data);
        }

        [HttpPost]
        public IActionResult CreateProject(ProjectCreateRequestModel project)
        {
                var result = _blProject.CreateProject(project);

                if (!result.IsSuccess) return BadRequest(result);
                return StatusCode(201, result.Data);
        }

        [HttpPut("{code}")]
        public IActionResult UpdateProject(string code, ProjectUpdateRequestModel project)
        {
                var result = _blProject.UpdateProject(code, project);

                if (!result.IsSuccess) return BadRequest(result);
                return StatusCode(204);
        }

        [HttpDelete("{code}")]
        public IActionResult DeleteProject(string code)
        {
                var result = _blProject.DeleteProject(code);

                if (result.IsSuccess) return BadRequest(result);
                return StatusCode(204);
        }
}
