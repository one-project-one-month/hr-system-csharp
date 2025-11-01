using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblLocation
{
    public string LocationId { get; set; } = null!;

    public string LocationCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Latitude { get; set; } = null!;

    public string Longitude { get; set; } = null!;

    public string Radius { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool DeleteFlag { get; set; }
}
