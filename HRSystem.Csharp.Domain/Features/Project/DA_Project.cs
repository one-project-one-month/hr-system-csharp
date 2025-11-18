using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared.Enums;
using Sprache;

namespace HRSystem.Csharp.Domain.Features.Project;

public class DA_Project
{
    private readonly AppDbContext _appDbContext;
    private readonly Generator _generator;
    private readonly DA_Sequence _daSequence;
    private readonly ILogger<DA_Project> _logger;

    public DA_Project(AppDbContext appDbContext, Generator generator, DA_Sequence daSequence,
        ILogger<DA_Project> logger)
    {
        _appDbContext = appDbContext;
        _generator = generator;
        _daSequence = daSequence;
        _logger = logger;
    }

    public async Task<Result<bool>> CreateProject(ProjectRequestModel project)
    {
        try
        {
            /*var lastProjectCode = await _appDbContext.TblProjects
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.ProjectCode)
                .FirstOrDefaultAsync();*/

            var newProject = project.Map();
            newProject.ProjectCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.PJ.ToString());

            _appDbContext.TblProjects.Add(newProject);
            var result = await _appDbContext.SaveChangesAsync();

            return result > 0
                ? Result<bool>.Success(true, "Project created successfully!")
                : Result<bool>.Error("Error creating project!");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"Error occured while creating project: {ex.Message}");
        }
    }

    public async Task<Result<ProjectListResponseModel>> GetAllProjects(ProjectListRequestModel reqModel)
    {
        try
        {
            var query = _appDbContext.TblProjects
                .AsNoTracking()
                .Where(r => !r.DeleteFlag);

            if (!string.IsNullOrWhiteSpace(reqModel.ProjectName))
            {
                query = query.Where(r => r.ProjectName.ToLower().Contains(reqModel.ProjectName.ToLower()));
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            var roles = query.Select(r => r.Map());

            var pagedResult = await roles.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new ProjectListResponseModel()
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<ProjectListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<ProjectListResponseModel>.Error($"Error occured while retrieving projects: {ex.Message}");
        }
    }

    public async Task<Result<ProjectResponseModel>> GetProjectByCode(ProjectEditRequestModel reqModel)
    {
        try
        {
            var project = await _appDbContext.TblProjects
                .AsNoTracking()
                .Where(p => p.DeleteFlag == false && p.ProjectCode == reqModel.ProjectCode)
                .Select(p => p.Map())
                .FirstOrDefaultAsync();

            return project is null
                ? Result<ProjectResponseModel>.NotFoundError("No project found!")
                : Result<ProjectResponseModel>.Success(project);
        }
        catch (Exception ex)
        {
            return Result<ProjectResponseModel>.Error($"Error occured while retrieving project: {ex.Message}");
        }
    }

    public async Task<Result<TblProject>> GetProjectByName(string name, string? excludeProjectCode = null)
    {
        try
        {
            var query = _appDbContext.TblProjects
                .AsNoTracking()
                .Where(p => !p.DeleteFlag && p.ProjectName == name);

            if (!string.IsNullOrWhiteSpace(excludeProjectCode))
            {
                query = query.Where(p => p.ProjectCode != excludeProjectCode);
            }

            var project = await query.FirstOrDefaultAsync();

            return project is not null
                ? Result<TblProject>.Success(project, "Project already exists!")
                : Result<TblProject>.NotFoundError("Project doesn't exist!");
        }
        catch (Exception ex)
        {
            return Result<TblProject>.Error($"Error occurred while checking project name: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateProject(string code, ProjectRequestModel project)
    {
        try
        {
            var existingProject = await _appDbContext.TblProjects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProjectCode == code && p.DeleteFlag == false);

            if (existingProject is null)
                return Result<bool>.NotFoundError("Project doesn't exist!");

            existingProject.ProjectName = project.ProjectName;
            existingProject.ProjectDescription = project.ProjectDescription;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.ProjectStatus = project.ProjectStatus.ToString();
            existingProject.ModifiedAt = DateTime.UtcNow;
            existingProject.ModifiedBy = "TestingUser";

            _appDbContext.TblProjects.Update(existingProject);
            var result = await _appDbContext.SaveChangesAsync();

            return result > 0
                ? Result<bool>.Success(true, "project updated success")
                : Result<bool>.Error("fail to update project!");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"Error occured while updating project: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteProject(string code)
    {
        try
        {
            var project = await _appDbContext.TblProjects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProjectCode == code && p.DeleteFlag == false);

            if (project is null)
            {
                return Result<bool>.NotFoundError("Project doesn't exist!");
            }

            project.DeleteFlag = true;

            _appDbContext.TblProjects.Update(project);
            var result = await _appDbContext.SaveChangesAsync();

            return result > 0
                ? Result<bool>.Success(true, "Project deleted successfully!")
                : Result<bool>.Error("Failed to delete project!");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"Error occured while deleting projects: {ex.Message}");
        }
    }

    public async Task<Result<AddEmployeeToProjectResponseModel>> CheckEmployeesAlreadyAssigned(
        string projectCode,
        AddEmployeeToProjectRequestModel reqModel)
    {
        var alreadyAddedEmployees = await _appDbContext.TblEmployeeProjects
            .AsNoTracking()
            .Where(e => !e.DeleteFlag
                        && e.ProjectCode == projectCode
                        && reqModel.EmployeeCodes.Contains(e.EmployeeCode))
            .Select(e => e.EmployeeCode)
            .ToListAsync();

        if (alreadyAddedEmployees.Count != 0)
        {
            return Result<AddEmployeeToProjectResponseModel>.ValidationError(
                $"Employees with code(s): {string.Join(", ", alreadyAddedEmployees)} are already added to Project - {projectCode}",
                new AddEmployeeToProjectResponseModel
                {
                    EmployeeCodes = alreadyAddedEmployees
                }
            );
        }

        return Result<AddEmployeeToProjectResponseModel>.Success(
            new AddEmployeeToProjectResponseModel
            {
                EmployeeCodes = new List<string>()
            },
            $"No duplicate employees found for Project - {projectCode}"
        );
    }

    public async Task<Result<AddEmployeeToProjectResponseModel>> AddEmployee(
        string projectCode,
        AddEmployeeToProjectRequestModel reqModel)
    {
        await using var tx = await _appDbContext.Database.BeginTransactionAsync();

        try
        {
            var entries = new List<TblEmployeeProject>(reqModel.EmployeeCodes.Count);
            foreach (var employeeCode in reqModel.EmployeeCodes)
            {
                var empPrjCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.EMP_PRJ.ToString());

                entries.Add(new TblEmployeeProject
                {
                    EmployeeProjectId = DevCode.GenerateNewUlid(),
                    EmployeeProjectCode = empPrjCode,
                    ProjectCode = projectCode,
                    EmployeeCode = employeeCode,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DeleteFlag = false
                });
            }

            await _appDbContext.TblEmployeeProjects.AddRangeAsync(entries);
            await _appDbContext.SaveChangesAsync();
            await tx.CommitAsync();

            return Result<AddEmployeeToProjectResponseModel>.Success(
                new AddEmployeeToProjectResponseModel { EmployeeCodes = reqModel.EmployeeCodes },
                $"Successfully added {reqModel.EmployeeCodes.Count} employee(s) to project {projectCode}."
            );
        }
        catch (DbUpdateException ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, $"Insert failed for project {projectCode}");
            return Result<AddEmployeeToProjectResponseModel>.ValidationError(
                $"Failed to add employees to project {projectCode}. No records were added."
            );
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, $"Unexpected error for adding employees to project {projectCode}");
            return Result<AddEmployeeToProjectResponseModel>.SystemError(
                "Unexpected error. No records were added."
            );
        }
    }
}