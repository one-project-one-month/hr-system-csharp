using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblMenuItem
{
    public string MenuItemId { get; set; } = null!;

    public string MenuItemCode { get; set; } = null!;

    public string MenuItemName { get; set; } = null!;

    public string? Description { get; set; }

    public string Url { get; set; } = null!;

    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public string MenuGroupCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
