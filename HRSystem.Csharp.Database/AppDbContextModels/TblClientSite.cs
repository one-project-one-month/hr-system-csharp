using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblClientSite
{
    public string ClientSiteId { get; set; } = null!;

    public string ClientSiteCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public decimal Radius { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool? DeleteFlag { get; set; }
}
