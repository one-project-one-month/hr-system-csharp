using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblRoleAndMenuPermission
{
    public string RoleAndMenuPermissionId { get; set; } = null!;

    public string? RoleAndMenuPermissionCode { get; set; }

    public string RoleCode { get; set; } = null!;

    public string? MenuGroupCode { get; set; }

    public string? MenuCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
