using HRSystem.Csharp.Domain.Models.Project;
using Microsoft.IdentityModel.Tokens;

namespace HRSystem.Csharp.Domain.Helpers;

public static class RequestValidator
{
    public static Result<bool> ValidateProject(ProjectRequestModel project)
    {
        if (project is null)
            return Result<bool>.BadRequestError("Project data is required");

        if (project.ProjectName.IsNullOrEmpty())
            return Result<bool>.ValidationError("Project name is required");

        if (!project.StartDate.HasValue)
            return Result<bool>.ValidationError("Start date is required");

        if (!project.EndDate.HasValue)
            return Result<bool>.ValidationError("End date is required");
        
        if (project.EndDate < project.StartDate)
            return Result<bool>.ValidationError("End date cannot be before start date.");

        return Result<bool>.Success(true);
    }

    public static Result<bool> ValidateCreateLocation(LocationCreateRequestModel location)
    {
        if (location is null)
            return Result<bool>.BadRequestError("Location data is required");

        if (location.LocationCode.IsNullOrEmpty())
            return Result<bool>.BadRequestError("Location code is required");

        if (location.Name.IsNullOrEmpty())
            return Result<bool>.BadRequestError("Location name is required");

        if (location.Latitude.IsNullOrEmpty())
            return Result<bool>.BadRequestError("Latitude is required");

        if (location.Longitude.IsNullOrEmpty())
            return Result<bool>.BadRequestError("Longitude is required");

        if (location.Radius.IsNullOrEmpty())
            return Result<bool>.BadRequestError("Radius is required");

        return Result<bool>.Success(true);
    }

    public static Result<bool> ValidateUpdateLocation(string locationCode, LocationUpdateRequestModel location)
    {
        if (location is null)
            return Result<bool>.BadRequestError("Location data is required");

        if (locationCode.IsNullOrEmpty())
            return Result<bool>.BadRequestError("Location code is required");

        return Result<bool>.Success(true);
    }
}