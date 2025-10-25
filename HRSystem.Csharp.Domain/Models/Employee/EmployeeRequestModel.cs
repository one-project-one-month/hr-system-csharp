namespace HRSystem.Csharp.Domain.Models.Employee;

public class EmployeeRequestModel
{

}

public class EmployeeCreateRequestModel
{
    public string? Username { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? RoleCode { get; set; }

    public string? Email { get; set; }

    public string? PhoneNo { get; set; }

    public decimal? Salary { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? ResignDate { get; set; }
}

public class EmployeeUpdateRequestModel
{
    public string? Name { get; set; }

    public string? RoleCode { get; set; }

    public string? Email { get; set; }

    public string? PhoneNo { get; set; }

    public decimal? Salary { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Resigndate { get; set; }
}