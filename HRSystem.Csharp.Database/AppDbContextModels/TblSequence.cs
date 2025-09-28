using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblSequence
{
    public Guid SequenceId { get; set; }

    public string? UniqueName { get; set; }

    public string? SequenceNo { get; set; }

    public DateTime? SequenceDate { get; set; }

    public string? SequenceType { get; set; }

    public string? RoleCode { get; set; }

    public bool? DeleteFlag { get; set; }
}
