using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Attendance
{
    public class DA_Attendance
    {
        private readonly AppDbContext _db;

        public DA_Attendance(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }

        public async Task<Result<AttendanceListResponseModel>> GetAllAttendances()
        {
            var attendanceList = await _db.TblAttendances.Where(x => x.DeleteFlag == false)
                .OrderByDescending(x => x.AttendanceDate)
                .ToListAsync();

            var model = new AttendanceListResponseModel
            {
                AttendanceList = attendanceList
                .Select(AttendanceListModel.FromTblAttendance)
                .ToList()
            };

        }
    }
}
