using HRSystem.Csharp.Domain.Models.Project;
using Sprache;

namespace HRSystem.Csharp.Domain.Features.Project;

public class BL_Project
{
    private readonly DA_Project _daProject;
    private readonly DA_Employee _daEmployee;
    private readonly ILogger<BL_Project> _logger;

    public BL_Project(DA_Project daProject, DA_Employee daEmployee, ILogger<BL_Project> logger)
    {
        _daProject = daProject;
        _daEmployee = daEmployee;
        _logger = logger;
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

    public async Task<Result<ProjectResponseModel>> GetProjectByCode(ProjectEditRequestModel reqModel)
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

    /*public async Task<Result<>> EmployeesAssignedToProject(string projectCode)
    {
        try
        {
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }*/

    public async Task<Result<AddEmployeeToProjectResponseModel>> AddEmployee(string projectCode,
        AddEmployeeToProjectRequestModel reqModel)
    {
        try
        {
            #region Validate Duplicate Employee Code

            var normalized = reqModel?.EmployeeCodes
                ?.Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .ToList() ?? new List<string>();

            if (!normalized.Any())
                return Result<AddEmployeeToProjectResponseModel>.ValidationError(
                    "At least one employee code is required.",
                    new AddEmployeeToProjectResponseModel { EmployeeCodes =  [] });

            // check duplicate code in request data
            var duplicatesInRequest = normalized
                .GroupBy(c => c, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatesInRequest.Any())
            {
                return Result<AddEmployeeToProjectResponseModel>.ValidationError(
                    $"Duplicate employee codes in request: {string.Join(", ", duplicatesInRequest)}",
                    new AddEmployeeToProjectResponseModel { EmployeeCodes = duplicatesInRequest });
            }

            #endregion

            #region Validate Project Exists

            var project = await GetProjectByCode(new ProjectEditRequestModel
            {
                ProjectCode = projectCode
            });

            if (project.IsError)
            {
                return Result<AddEmployeeToProjectResponseModel>.SystemError(project.Message!);
            }

            if (project?.Data is null)
            {
                return Result<AddEmployeeToProjectResponseModel>.NotFoundError(
                    $"Project - {projectCode} doesn't exist!");
            }

            #endregion

            #region Validate each employee exists and not have been added to the project

            // check employee exist in Tbl_Employee
            var employeesExistResult = await _daEmployee.ValidateEmployeesExist(reqModel!);
            if (employeesExistResult.IsError)
            {
                return employeesExistResult;
            }

            // check employees already added to the project
            var assignedEmpRes = await _daProject.CheckEmployeesAlreadyAssigned(projectCode, reqModel!);
            if (assignedEmpRes.IsError)
            {
                return assignedEmpRes;
            }

            #endregion

            var result = await _daProject.AddEmployee(projectCode, reqModel!);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString(), "Error adding employees to project");
            return Result<AddEmployeeToProjectResponseModel>.SystemError("Error adding employees to project");
        }
    }

    public async Task<Result<AddEmployeeToProjectResponseModel>> RemoveEmployee(string projectCode,
        AddEmployeeToProjectRequestModel reqModel)
    {
        try
        {
            #region Validate Project Exists

            var project = await GetProjectByCode(new ProjectEditRequestModel
            {
                ProjectCode = projectCode
            });

            if (project.IsError)
            {
                return Result<AddEmployeeToProjectResponseModel>.SystemError(project.Message!);
            }

            if (project?.Data is null)
            {
                return Result<AddEmployeeToProjectResponseModel>.NotFoundError(
                    $"Project - {projectCode} doesn't exist!");
            }

            #endregion

            #region Validate each employee exists and not have been added to the project

            var invalidEmployeesResult = await _daEmployee.ValidateEmployeesExist(reqModel);
            if (invalidEmployeesResult.IsError)
            {
                return invalidEmployeesResult;
            }

            #endregion

            return await _daProject.RemoveEmployee(projectCode, reqModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString(), "Error removing employees from project.");
            return Result<AddEmployeeToProjectResponseModel>.SystemError("Error removing employees from project");
        }
    }
}