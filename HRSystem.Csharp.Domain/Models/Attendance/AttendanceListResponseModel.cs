using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Attendance
{
    public class AttendanceListResponseModel
    {
        public List<AttendanceListModel> AttendanceList { get; set; } = new List<AttendanceListModel>();
    }

    public class AttendanceListModel
    {
        public string AttendanceId { get; set; }

        public string? AttendanceCode { get; set; }

        public string? EmployeeCode { get; set; }

        public DateTime? AttendanceDate { get; set; }

        public DateTime? CheckInTime { get; set; }

        public string? CheckInLocation { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public string? CheckOutLocation { get; set; }

        public decimal? WorkingHour { get; set; }

        public string? Status { get; set; }

        public bool? IsSavedLocation { get; set; }

        public static AttendanceListModel FromTblAttendance(TblAttendance attendance)
        {
            return new AttendanceListModel()
            {
                AttendanceId = attendance.AttendanceId,
                AttendanceCode = attendance.AttendanceCode,
                EmployeeCode = attendance.EmployeeCode,
                AttendanceDate = attendance.AttendanceDate,
                CheckInTime = attendance.CheckInTime,
                CheckInLocation = attendance.CheckInLocation,
                CheckOutTime = attendance.CheckOutTime,
                CheckOutLocation = attendance.CheckOutLocation,
                WorkingHour = attendance.WorkingHour,
                IsSavedLocation = attendance.IsSavedLocation
            };
        }
    }
}