using FluentValidation;
using HRSystem.Csharp.Domain.Helpers;
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared;

namespace HRSystem.Csharp.Domain.Features.Project;

public class BL_Project
{
        private readonly DA_Project _daProject;
        private readonly IValidator<ProjectCreateRequestModel> _projCreateValidator;
        private readonly IValidator<ProjectUpdateRequestModel> _projUpdateValidator;

        public BL_Project(DA_Project daProject, 
                IValidator<ProjectCreateRequestModel> projectCreateValidator,
                IValidator<ProjectUpdateRequestModel> projectUpdateValidator)
        {
                _daProject = daProject;
                _projCreateValidator = projectCreateValidator;
                _projUpdateValidator = projectUpdateValidator;
        }

        public Result<Boolean> CreateProject(ProjectCreateRequestModel project)
        {
                var validator = _projCreateValidator.Validate(project);
                if (!validator.IsValid) 
                        return Result<Boolean>.InvalidDataError(validator.Errors.FirstOrDefault().ErrorMessage!);

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
                 var validator = _projUpdateValidator.Validate(project);
                 if (!validator.IsValid) 
                        return Result<Boolean>.InvalidDataError(validator.Errors.FirstOrDefault().ErrorMessage!);

                return _daProject.UpdateProject(code, project);
        }
}
