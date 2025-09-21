using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblEmployeeShift
{
    public string EmployeeShiftId { get; set; } = null!;

    public string EmployeeShiftCode { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public string ShiftCode { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}
