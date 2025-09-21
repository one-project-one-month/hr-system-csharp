using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblPermission
{
    public string PermissionId { get; set; } = null!;

    public string PermissionCode { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public string? MenuCode { get; set; }

    public string? MenuGroupCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
