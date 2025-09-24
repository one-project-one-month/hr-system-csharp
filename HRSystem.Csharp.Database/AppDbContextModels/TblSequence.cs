using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblSequence
{
    public string SequenceId { get; set; } = null!;

    public string UniqueName { get; set; } = null!;

    public string? SequenceNo { get; set; }

    public DateTime? SequenceDate { get; set; }

    public string? SequenceType { get; set; }

    public string? RoleCode { get; set; }

    public bool? DeleteFlag { get; set; }
}
