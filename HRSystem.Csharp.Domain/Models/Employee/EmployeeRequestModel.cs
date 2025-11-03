using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Domain.Models.Employee;

public class EmployeeRequestModel
{

}

public class EmployeeListRequestModel: PaginationRequestModel
{
    public string EmployeeName { get; set; }
}

public class UserProfileRequestModel
{
    public string EmployeeCode { get; set; }
}
public class EmployeeCreateRequestModel
{
    public string Username { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RoleCode { get; set; }
    public string PhoneNo { get; set; }
    public decimal Salary { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ResignDate { get; set; }
}

public class EmployeeUpdateRequestModel
{
    public string Name { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? ResignDate { get; set; }
}