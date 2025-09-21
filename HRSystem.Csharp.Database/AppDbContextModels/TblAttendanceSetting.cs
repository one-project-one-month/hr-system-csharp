using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblAttendanceSetting
{
    public string ShiftId { get; set; } = null!;

    public string ShiftCode { get; set; } = null!;

    public TimeOnly DefaultCheckInTime { get; set; }

    public TimeOnly DefaultCheckOutTime { get; set; }

    public int? GracePeriod { get; set; }
}
