using HRSystem.Csharp.Domain.Models.Project;
using Sprache;

namespace HRSystem.Csharp.Domain.Features.Project;

public class BL_Project
{
    private readonly DA_Project _daProject;
    private readonly DA_Employee _daEmployee;

    public BL_Project(DA_Project daProject, DA_Employee daEmployee)
    {
        _daProject = daProject;
        _daEmployee = daEmployee;
    }

    public async Task<Result<ProjectListResponseModel>> GetAllProjects(ProjectListRequestModel reqModel)
    {
        return await _daProject.GetAllProjects(reqModel);
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

    public async Task<Result<ProjectResponseModel>> GetProject(ProjectEditRequestModel reqModel)
    {
        return await _daProject.GetProjectByCode(reqModel);
    }

    public async Task<Result<bool>> UpdateProject(string projectCode, ProjectRequestModel project)
    {
        var validation = RequestValidator.ValidateProject(project);

        if (!validation.IsSuccess)
            return Result<bool>.BadRequestError(validation.Message!);

        var existing = await _daProject.GetProjectByName(project.ProjectName, projectCode);
        if (existing is { IsSuccess: true, Data: not null })
        {
            return Result<bool>.DuplicateRecordError("Project name already exists!");
        }

        return await _daProject.UpdateProject(projectCode, project);
    }

    public async Task<Result<bool>> DeleteProject(string code)
    {
        return await _daProject.DeleteProject(code);
    }

    public async Task<Result<AddEmployeeToProjectResponseModel>> AddEmployee(string projectCode,
        AddEmployeeToProjectRequestModel reqModel)
    {
        try
        {
            var project = await GetProject(new ProjectEditRequestModel
            {
                ProjectCode = projectCode
            });

            if (project.IsError || project?.Data is null)
            {
                return Result<AddEmployeeToProjectResponseModel>.NotFoundError(
                    $"Project - {projectCode} doesn't exist!");
            }

            // check employee exist in Tbl_Employee
            var invalidEmployeesResult = await _daEmployee.ValidateEmployeesExist(reqModel);
            if (invalidEmployeesResult.IsError)
            {
                return invalidEmployeesResult;
            }

            // check employees already added to the project
            var assignedEmpRes = await _daProject.CheckEmployeesAlreadyAssigned(projectCode, reqModel);
            if (assignedEmpRes.IsError)
            {
                return assignedEmpRes;
            }

            var result = await _daProject.AddEmployee(projectCode, reqModel);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}