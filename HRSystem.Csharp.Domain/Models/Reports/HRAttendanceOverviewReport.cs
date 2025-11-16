using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Reports
{
    public class HRAttendanceOverviewReport
    {
        public HRAttendanceOverviewReport() { }
        public Int32 Present { get; set; } = 0;
        public Int32 Absent { get; set; } = 0;
        public Int32 Late { get; set; } = 0;
        public Int32 EmpCount { get; set; } = 0;
    }
}
