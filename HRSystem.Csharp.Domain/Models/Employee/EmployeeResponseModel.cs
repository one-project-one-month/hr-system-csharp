namespace HRSystem.Csharp.Domain.Models.Employee;

public class EmployeeListResponseModel: PagedResult<EmployeeResponseModel>
{
}

public class EmployeeResponseModel
{
    public string? ProfileImage { get; set; }

    public string? EmployeeCode { get; set; }

    public string? Username { get; set; }

    public string? RoleName { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PhoneNo { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class EmployeeEditResponseModel
{
    public string? EmployeeCode { get; set; }
    public string? Username { get; set; }
    public string? RoleCode { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNo { get; set; }
    public decimal? Salary { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ResignDate { get; set; }
}

public class EmployeeCreateResponseModel
{
}

public class EmployeeUpdateResponseModel
{
}

public class EmployeeDeleteResponseModel
{
}