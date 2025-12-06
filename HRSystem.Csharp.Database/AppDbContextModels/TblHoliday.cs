using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblHoliday
{
    public Guid HolidayId { get; set; }

    public DateOnly HolidayDate { get; set; }

    public string? Description { get; set; }

    public bool? IsWorkingHoliday { get; set; }
}
