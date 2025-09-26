using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblSequence
{
    public string SequenceId { get; set; } = null!;

    public string UniqueName { get; set; } = null!;

    public string SequenceNo { get; set; } = null!;

    public DateTime SequenceDate { get; set; }

    public string SequenceType { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public bool? DeleteFlag { get; set; }
}
