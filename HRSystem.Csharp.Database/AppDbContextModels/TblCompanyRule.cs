using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblCompanyRule
{
    public string CompanyRuleId { get; set; } = null!;

    public string CompanyRuleCode { get; set; } = null!;

    public string? Description { get; set; }

    public string? Value { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
