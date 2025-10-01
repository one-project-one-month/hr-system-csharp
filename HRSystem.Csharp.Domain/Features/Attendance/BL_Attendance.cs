using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Attendance
{
    public class BL_Attendance
    {
        private readonly DA_Attendance _attendance;

        public BL_Attendance(DA_Attendance attendance)
        {
            _attendance = attendance;
        }

        public async Task<Result<AttendanceListResponseModel>> GetAllAttendances()
        {
            var data = await _attendance.GetAllAttendances();
            return data;
        }
    }
}
