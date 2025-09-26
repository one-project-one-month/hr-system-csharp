﻿using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblRole
{
    public string RoleId { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public string? RoleName { get; set; }

    public string? UniqueName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
