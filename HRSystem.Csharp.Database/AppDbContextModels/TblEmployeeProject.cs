using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblEmployeeProject
{
    public string EmployeeProjectId { get; set; } = null!;

    public string EmployeeProjectCode { get; set; } = null!;

    public string? ProjectCode { get; set; }

    public string? EmployeeCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
