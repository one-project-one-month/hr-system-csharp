using HRSystem.Csharp.Domain.Models.Project;

namespace HRSystem.Csharp.Domain.Features.Project;

public class BL_Project
{
    private readonly DA_Project _daProject;

    public BL_Project(DA_Project daProject)
    {
        _daProject = daProject;
    }

    public async Task<Result<bool>> CreateProject(ProjectRequestModel project)
    {
        var validation = RequestValidator.ValidateProject(project);
        if (!validation.IsSuccess)
            return Result<bool>.BadRequestError(validation.Message!);

        var existing = await _daProject.GetProjectByName(project.ProjectName);
        if (existing is { IsSuccess: true, Data: not null })
        {
            return Result<bool>.DuplicateRecordError("Project name already exists!");
        }

        return await _daProject.CreateProject(project);
    }

    public async Task<Result<ProjectListResponseModel>> GetAllProjects(ProjectListRequestModel reqModel)
    {
        return await _daProject.GetAllProjects(reqModel);
    }

    public async Task<Result<ProjectResponseModel>> GetProject(string code)
    {
        return await _daProject.GetProjectByCode(code);
    }

    public async Task<Result<bool>> DeleteProject(string code)
    {
        return await _daProject.DeleteProject(code);
    }

    public async Task<Result<bool>> UpdateProject(string code, ProjectRequestModel project)
    {
        var validation = RequestValidator.ValidateProject(project);

        if (!validation.IsSuccess)
            return Result<bool>.BadRequestError(validation.Message!);

        return await _daProject.UpdateProject(code, project);
    }
}