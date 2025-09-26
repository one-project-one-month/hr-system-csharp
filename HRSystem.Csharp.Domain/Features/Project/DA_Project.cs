
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared;

namespace HRSystem.Csharp.Domain.Features.Project;

public class DA_Project
{
        private readonly AppDbContext _appDbContext;

        public DA_Project(AppDbContext appDbContext)
        {
                _appDbContext = appDbContext;
        }

        public Result<Boolean> CreateProject(ProjectCreateRequestModel project)
        {
                try
                {
                        var newProject = new TblProject
                        {
                                ProjectId = Ulid.NewUlid().ToString(),
                                ProjectCode = project.ProjectCode,
                                ProjectName = project.ProjectName,
                                ProjectDescription = project.ProjectDescription,
                                StartDate = project.StartDate,
                                EndDate = project.EndDate,
                                ProjectStatus = "Pending",
                                CreatedAt = DateTime.Now,
                                CreatedBy = "TestingUser",
                                DeleteFlag = false
                        };

                        _appDbContext.TblProjects.Add(newProject);
                        var result = _appDbContext.SaveChanges();

                        return result > 0 ? Result<Boolean>.Success(true, "project created success")
                                : Result<Boolean>.Error("fail to create project!");
                }
                catch (Exception ex)
                {

                        return Result<Boolean>.Error($"Error occured while creating project: {ex.Message}");
                }
        }

        public Result<List<ProjectResponseModel>> GetAllProjects()
        {
                try
                {
                        var projects = _appDbContext.TblProjects
                                .Where(p => p.DeleteFlag == false)
                                .Select(p => new ProjectResponseModel
                                {
                                        ProjectCode = p.ProjectCode,
                                        ProjectName = p.ProjectName,
                                        ProjectDescription = p.ProjectDescription,
                                        StartDate = p.StartDate,
                                        EndDate = p.EndDate,
                                        ProjectStatus = p.ProjectStatus,
                                        CreatedAt = p.CreatedAt,
                                        CreatedBy = p.CreatedBy,
                                        ModifiedAt = p.ModifiedAt,
                                        ModifiedBy = p.ModifiedBy,
                                }).ToList();

                        if (projects is null || projects.Count is 0) 
                                return Result<List<ProjectResponseModel>>.NotFoundError("no projects found!");

                        return Result<List<ProjectResponseModel>>.Success(projects);

                }
                catch (Exception ex)
                {

                        return Result<List<ProjectResponseModel>>.Error($"Error occured while retreving projects: {ex.Message}");
                }
        }

        public Result<ProjectResponseModel> GetProjectByCode(string code)
        {
                try
                {
                        var project = _appDbContext.TblProjects
                                .Where(p => p.DeleteFlag == false)
                                .Select(p => new ProjectResponseModel
                                {
                                        ProjectCode = p.ProjectCode,
                                        ProjectName = p.ProjectName,
                                        ProjectDescription = p.ProjectDescription,
                                        StartDate = p.StartDate,
                                        EndDate = p.EndDate,
                                        ProjectStatus = p.ProjectStatus,
                                        CreatedAt = p.CreatedAt,
                                        CreatedBy = p.CreatedBy,
                                        ModifiedAt = p.ModifiedAt,
                                        ModifiedBy = p.ModifiedBy,
                                }).FirstOrDefault(p => p.ProjectCode == code);

                        if (project is null) return Result<ProjectResponseModel>.NotFoundError("no project found!");

                        return Result<ProjectResponseModel>.Success(project);
                }
                catch (Exception ex)
                {

                        return Result<ProjectResponseModel>.Error($"Error occured while retreving project: {ex.Message}");
                }
        }

        public Result<Boolean> UpdateProject(string code, ProjectUpdateRequestModel project)
        {
                try
                {
                        var existingProject = _appDbContext.TblProjects
                                .FirstOrDefault(p => p.ProjectCode == code && p.DeleteFlag == false);

                        if (existingProject is null) 
                                return Result<Boolean>.NotFoundError("no project found to update!");

                        existingProject.ProjectName = project.ProjectName;
                        existingProject.ProjectDescription = project.ProjectDescription;
                        existingProject.StartDate = project.StartDate;
                        existingProject.EndDate = project.EndDate;
                        existingProject.ProjectStatus = project.ProjectStatus;
                        existingProject.ModifiedAt = DateTime.Now;
                        existingProject.ModifiedBy = "TestingUser";

                        _appDbContext.TblProjects.Update(existingProject);
                        var result = _appDbContext.SaveChanges();

                        return result > 0 ? Result<Boolean>.Success(true, "project updated success") 
                                : Result<Boolean>.Error("fail to update project!");

                }
                catch (Exception ex)
                {

                        return Result<Boolean>.Error($"Error occured while updating project: {ex.Message}\"");
                }
        }

        public Result<Boolean> DeleteProject(string code)
        {
                try
                {
                        var project = _appDbContext.TblProjects
                                .FirstOrDefault(p => p.ProjectCode == code && p.DeleteFlag == false);

                        if (project is null) return Result<Boolean>.NotFoundError("no project found to delete!");

                        project.DeleteFlag = true;

                        _appDbContext.TblProjects.Update(project);
                        var result = _appDbContext.SaveChanges();

                        return result > 0 ? Result<Boolean>.Success(true, "project deleted success.") 
                                : Result<Boolean>.Error("fail to delete project!");

                }
                catch (Exception ex)
                {

                        return Result<Boolean>.Error($"Error occured while deleting projects: {ex.Message}");
                }
        }

}
