using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblEmployee
{
    public Guid EmployeeId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? RoleCode { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? PhoneNo { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? ResignDate { get; set; }

    public decimal? Salary { get; set; }

    public bool? IsFirstTimeLogin { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
