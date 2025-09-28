using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblMenu
{
    public Guid MenuId { get; set; }

    public string? MenuCode { get; set; }

    public string? MenuGroupCode { get; set; }

    public string? MenuName { get; set; }

    public string? Url { get; set; }

    public string? Icon { get; set; }

    public int? SortOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
