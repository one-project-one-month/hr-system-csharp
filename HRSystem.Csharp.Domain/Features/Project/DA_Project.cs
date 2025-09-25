
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

                return result > 0 ? Result<Boolean>.Success(true)
                        : Result<Boolean>.Error();
        }

        public Result<List<ProjectResponseModel>> GetAllProjects()
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
                        return Result<List<ProjectResponseModel>>.NotFoundError();

                return Result<List<ProjectResponseModel>>.Success(projects);
        }

        public Result<ProjectResponseModel> GetProjectByCode(string code)
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

                if (project is null) return Result<ProjectResponseModel>.NotFoundError();

                return Result<ProjectResponseModel>.Success(project);
        }

        public Result<Boolean> UpdateProject(string code, ProjectUpdateRequestModel project)
        {
                var existingProject = _appDbContext.TblProjects.FirstOrDefault(p => p.ProjectCode == code);

                if (existingProject is null) return Result<Boolean>.NotFoundError();

                existingProject.ProjectName = project.ProjectName;
                existingProject.ProjectDescription = project.ProjectDescription;
                existingProject.StartDate = project.StartDate;
                existingProject.EndDate = project.EndDate;
                existingProject.ProjectStatus = project.ProjectStatus;
                existingProject.ModifiedAt = DateTime.Now;
                existingProject.ModifiedBy = "TestingUser";

                _appDbContext.TblProjects.Update(existingProject);
                var result = _appDbContext.SaveChanges();

                return result > 0 ? Result<Boolean>.Success(true) : Result<Boolean>.Error();
        }

        public Result<Boolean> DeleteProject(string code)
        {
                var project = _appDbContext.TblProjects.FirstOrDefault(p => p.ProjectCode == code);

                if (project is null) return Result<Boolean>.NotFoundError();

                project.DeleteFlag = true;

                _appDbContext.TblProjects.Update(project);
                var result = _appDbContext.SaveChanges();

                return result > 0 ? Result<Boolean>.Success(true) : Result<Boolean>.Error();
        }

}
