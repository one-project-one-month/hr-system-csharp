using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblRoleAndMenuPermission
{
    public Guid RoleAndMenuPermissionId { get; set; }

    public string? RoleAndMenuPermissionCode { get; set; }

    public string? RoleCode { get; set; }

    public string? MenuGroupCode { get; set; }

    public string? MenuCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
