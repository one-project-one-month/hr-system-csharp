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
            return Result<bool>.BadRequestError("Project name is required");

        if (!project.StartDate.HasValue)
            return Result<bool>.BadRequestError("Start date is required");

        if (!project.EndDate.HasValue)
            return Result<bool>.BadRequestError("End date is required");

        return Result<bool>.Success(true);
    }
}