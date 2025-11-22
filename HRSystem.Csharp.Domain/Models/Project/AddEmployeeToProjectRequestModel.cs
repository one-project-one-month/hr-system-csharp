namespace HRSystem.Csharp.Domain.Models.Project;

public class AddEmployeeToProjectRequestModel
{
    public List<string> EmployeeCodes { get; set; } = new();
}

public class AddEmployeeToProjectResponseModel
{
    public List<string> EmployeeCodes { get; set; } = new();
}