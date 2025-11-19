using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblVerification
{
    public string VerificationId { get; set; } = null!;

    public string VerificationCode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? ExpiredTime { get; set; }

    public bool? IsUsed { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; }
}
