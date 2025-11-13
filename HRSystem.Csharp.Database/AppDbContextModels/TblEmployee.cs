using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblEmployee
{
    public string EmployeeId { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public string ProfileImage { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? ResignDate { get; set; }

    public decimal Salary { get; set; }

    public bool IsFirstTimeLogin { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
