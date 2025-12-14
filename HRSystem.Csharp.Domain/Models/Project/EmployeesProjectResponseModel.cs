using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Domain.Models.Project;

public class EmployeesProjectRequestModel : PaginationRequestModel
{
}

public class EmployeesProjectResponseModel
{
    public string ProjectCode { get; set; }
    public PagedResult<EmployeesInfo> EmployeeList { get; set; }
}

public class EmployeesInfo
{
    public string EmployeeCode { get; set; }
    public string EmployeeName { get; set; }
}