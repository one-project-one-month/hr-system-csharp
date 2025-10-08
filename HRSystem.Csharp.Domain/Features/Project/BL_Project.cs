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

        public Result<Boolean> CreateProject(ProjectCreateRequestModel project)
        {
                return _daProject.CreateProject(project);
        }

        public Result<List<ProjectResponseModel>> GetAllProjects()
        {
                return _daProject.GetAllProjects();
        }

        public Result<ProjectResponseModel> GetProject(string code)
        {
                return _daProject.GetProjectByCode(code);
        }

        public Result<Boolean> DeleteProject(string code)
        {
                return _daProject.DeleteProject(code);
        }

        public Result<Boolean> UpdateProject(string code, ProjectUpdateRequestModel project)
        {
                return _daProject.UpdateProject(code, project);
        }
}
