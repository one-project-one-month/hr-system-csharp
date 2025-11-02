using HRSystem.Csharp.Domain.Models.Project;

namespace HRSystem.Csharp.Domain.Features.Project;

public class DA_Project
{
    private readonly AppDbContext _appDbContext;
    private readonly Generator _generator;

    public DA_Project(AppDbContext appDbContext, Generator generator)
    {
        _appDbContext = appDbContext;
        _generator = generator;
    }

    public async Task<Result<bool>> CreateProject(ProjectRequestModel project)
    {
        try
        {
            var lastProjectCode = await _appDbContext.TblProjects
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.ProjectCode)
                .FirstOrDefaultAsync();

            var newProject = project.Map();
            newProject.ProjectCode = _generator.GenerateProjectCode(lastProjectCode);

            /*var existingProject =
                await _appDbContext.TblProjects.FirstOrDefaultAsync(p => p.ProjectCode == newProject.ProjectCode);

            if (existingProject is not null)
                return Result<bool>.DuplicateRecordError(
                    $"A project with code '{newProject.ProjectCode}' already exists!");*/

            _appDbContext.TblProjects.Add(newProject);
            var result = await _appDbContext.SaveChangesAsync();

            return result > 0
                ? Result<bool>.Success(true, "project created success")
                : Result<bool>.Error("fail to create project!");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"Error occured while creating project: {ex.Message}");
        }
    }

    public async Task<Result<List<ProjectResponseModel>>> GetAllProjects(ProjectListRequestModel reqModel)
    {
        try
        {
            var query = _appDbContext.TblProjects
                .AsNoTracking()
                .Where(r => !r.DeleteFlag);

            if (!string.IsNullOrWhiteSpace(reqModel.ProjectName))
            {
                query = query.Where(r => r.ProjectName.ToLower() == reqModel.ProjectName.ToLower());
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            var roles = query.Select(r => r.ma);

            var pagedResult = await roles.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new RoleListResponseModel
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<ProjectListRequestModel>.Success(result);
            
            /*var projects = await _appDbContext.TblProjects
                .AsNoTracking()
                .Where(p => p.DeleteFlag == false)
                .Select(p => p.Map())
                .ToListAsync();

            if (projects is null || projects.Count is 0)
                return Result<List<ProjectResponseModel>>.NotFoundError("no projects found!");

            return Result<List<ProjectResponseModel>>.Success(projects);*/
        }
        catch (Exception ex)
        {
            return Result<List<ProjectResponseModel>>.Error($"Error occured while retreving projects: {ex.Message}");
        }
    }

    public async Task<Result<ProjectResponseModel>> GetProjectByCode(string code)
    {
        try
        {
            var project = await _appDbContext.TblProjects
                .AsNoTracking()
                .Where(p => p.DeleteFlag == false && p.ProjectCode == code)
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

    public async Task<Result<TblProject>> GetProjectByName(string Name)
    {
        try
        {
            var project = await _appDbContext.TblProjects
                .AsNoTracking()
                .Where(p => p.DeleteFlag == false && p.ProjectName == Name)
                .FirstOrDefaultAsync();

            return project is null
                ? Result<TblProject>.NotFoundError("No project found!")
                : Result<TblProject>.Success(project);
        }
        catch (Exception ex)
        {
            return Result<TblProject>.Error($"Error occured while retreving project: {ex.Message}");
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
                return Result<bool>.NotFoundError("no project found to delete!");
            }

            project.DeleteFlag = true;

            _appDbContext.TblProjects.Update(project);
            var result = await _appDbContext.SaveChangesAsync();

            return result > 0
                ? Result<bool>.Success(true, "project deleted success.")
                : Result<bool>.Error("fail to delete project!");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"Error occured while deleting projects: {ex.Message}");
        }
    }
}