
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared;
using HRSystem.Csharp.Shared.Helpers;

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
                        var existingProject = _appDbContext.TblProjects.FirstOrDefault(p => p.ProjectCode == project.ProjectCode);

                        if (existingProject is not null)
                                return Result<Boolean>.DuplicateRecordError( $"A project with code '{project.ProjectCode}' already exists!");

                        var newProject = project.Map();

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
                                .Select(p => p.Map())
                                .ToList();

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
                                .Where(p => p.DeleteFlag == false &&  p.ProjectCode == code)
                                .Select(p => p.Map())
                                .FirstOrDefault();

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
                        existingProject.ProjectStatus = project.ProjectStatus.ToString();
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
