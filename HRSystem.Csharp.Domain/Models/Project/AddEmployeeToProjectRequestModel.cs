namespace HRSystem.Csharp.Domain.Models.Project;

public class AddEmployeeToProjectRequestModel
{
    public List<string> EmployeeCodes { get; set; } = new();
}

public class AddEmployeeToProjectResponseModel
{
    public List<string> EmployeeCodes { get; set; } = new();
}

public class ProjectOverviewListResponseModel
{
    public List<ProjectOverviewResponseModel> ProjectOverview { get; set; }
}

public class ProjectOverviewResponseModel
{
    public string ProjectStatus { get; set; }
    public int StatusCount { get; set; }
    public double Percentage { get; set; }
}