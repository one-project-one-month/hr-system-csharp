using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared.Enums;

namespace HRSystem.Csharp.Shared.Helpers;

// To impelemnt map functions between classes and tbls
public static class Mapper
{
        public static TblProject Map(this ProjectRequestModel project)
        {
                return new TblProject
                {
                        ProjectId = Ulid.NewUlid().ToString(),
                       // ProjectCode = project.ProjectCode,
                        ProjectName = project.ProjectName,
                        ProjectDescription = project.ProjectDescription,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate,
                        ProjectStatus = project.ProjectStatus.ToString(),
                        CreatedAt = DateTime.Now,
                        CreatedBy = "TestingUser",
                        DeleteFlag = false
                };
        }

        public static ProjectResponseModel Map(this TblProject project)
        {
                return new ProjectResponseModel
                {
                        ProjectCode = project.ProjectCode,
                        ProjectName = project.ProjectName,
                        ProjectDescription = project.ProjectDescription,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate,
                        ProjectStatus = (EnumProjectStatus)Enum.Parse(typeof(EnumProjectStatus), project.ProjectStatus!),
                        CreatedAt = project.CreatedAt,
                        CreatedBy = project.CreatedBy,
                        ModifiedAt = project.ModifiedAt,
                        ModifiedBy = project.ModifiedBy,
                };
        }

}
