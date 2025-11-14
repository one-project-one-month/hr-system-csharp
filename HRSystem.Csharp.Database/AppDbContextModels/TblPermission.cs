using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblPermission
{
    public string PermissionId { get; set; } = null!;

    public string PermissionCode { get; set; } = null!;

    public string PermissionName { get; set; } = null!;
}
