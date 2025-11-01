namespace HRSystem.Csharp.Domain.Models.Attendance;

public class AttendanceEditResponseModel
{
    public AttendanceEditModel? Attendance { get; set; }
}

public class AttendanceEditModel
{
    public string? AttendanceCode { get; set; }

    public string? EmployeeCode { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public DateTime? CheckInTime { get; set; }

    public string? CheckInLocation { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? CheckOutLocation { get; set; }

    public decimal? WorkingHour { get; set; }

    public int? HourLateFlag { get; set; }

    public int? HalfDayFlag { get; set; }

    public int? FullDayFlag { get; set; }

    public string? Remark { get; set; }

    public bool? IsSavedLocation { get; set; }

    public static AttendanceEditModel FromTblAttendance(TblAttendance tblAttendance)
    {
        var attendanceModel = new AttendanceEditModel()
        {
            AttendanceCode = tblAttendance.AttendanceCode,
            EmployeeCode = tblAttendance.EmployeeCode,
            AttendanceDate = tblAttendance.AttendanceDate,
            CheckInTime = tblAttendance.CheckInTime,
            CheckInLocation = tblAttendance.CheckInLocation,
            CheckOutTime = tblAttendance.CheckOutTime,
            CheckOutLocation = tblAttendance.CheckOutLocation,
            WorkingHour = tblAttendance.WorkingHour,
            HourLateFlag = tblAttendance.HourLateFlag,
            HalfDayFlag = tblAttendance.HalfDayFlag,
            FullDayFlag = tblAttendance.FullDayFlag,
            Remark = tblAttendance.Remark,
            IsSavedLocation = tblAttendance.IsSavedLocation
        };
        return attendanceModel;
    }
}