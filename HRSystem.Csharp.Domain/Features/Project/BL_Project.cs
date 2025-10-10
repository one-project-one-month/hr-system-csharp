using HRSystem.Csharp.Domain.Helpers;
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared;

namespace HRSystem.Csharp.Domain.Features.Project;

public class BL_Project
{
        private readonly DA_Project _daProject;

        public BL_Project(DA_Project daProject)
        {
                _daProject = daProject;
        }

        public async Task<Result<Boolean>> CreateProject(ProjectRequestModel project)
        {
                var validation = RequestValidator.ValidateProject(project);


                if (!validation.IsSuccess)
                        return Result<Boolean>.BadRequestError(validation.Message!);

                return await _daProject.CreateProject(project);
        }

        public async Task<Result<List<ProjectResponseModel>>> GetAllProjects()
        {
                return await _daProject.GetAllProjects();
        }

        public async Task<Result<ProjectResponseModel>> GetProject(string code)
        {
                return await _daProject.GetProjectByCode(code);
        }

        public async Task<Result<Boolean>> DeleteProject(string code)
        {
                return await _daProject.DeleteProject(code);
        }

        public async Task<Result<Boolean>> UpdateProject(string code, ProjectRequestModel project)
        {
                var validation = RequestValidator.ValidateProject(project);

                if (!validation.IsSuccess)
                        return Result<Boolean>.BadRequestError(validation.Message!);

                return await _daProject.UpdateProject(code, project);
        }
}
