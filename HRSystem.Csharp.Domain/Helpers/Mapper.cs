using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared.Enums;

namespace HRSystem.Csharp.Domain.Helpers;

public static class Mapper
{
    public static TblProject Map(this ProjectRequestModel project)
    {
        return new TblProject
        {
            ProjectId = Ulid.NewUlid().ToString(),
            ProjectName = project.ProjectName,
            ProjectDescription = project.ProjectDescription,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectStatus = project.ProjectStatus.ToString(),
            CreatedAt = DateTime.UtcNow,
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

    public static TblLocation Map(this LocationCreateRequestModel location)
    {
        return new TblLocation
        {
            LocationId = Ulid.NewUlid().ToString(),
            LocationCode = location.LocationCode,
            Name = location.Name,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Radius = location.Radius,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "system",
            DeleteFlag = false
        };
    }

    public static LocationResponseModel Map(this TblLocation location)
    {
        return new LocationResponseModel
        {
            LocationCode = location.LocationCode,
            Name = location.Name,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Radius = location.Radius,
            CreatedAt = location.CreatedAt,
            CreatedBy = location.CreatedBy,
            ModifiedAt = location.ModifiedAt,
            ModifiedBy = location.ModifiedBy,
            DeleteFlag = location.DeleteFlag
        };
    }
}