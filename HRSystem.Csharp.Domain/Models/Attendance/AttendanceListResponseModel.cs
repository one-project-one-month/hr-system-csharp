namespace HRSystem.Csharp.Domain.Models.Attendance;

public class AttendanceListResponseModel
{
    public List<AttendanceListModel> AttendanceList { get; set; } = new List<AttendanceListModel>();
}

public class AttendanceListModel
{
    public string AttendanceId { get; set; }

    public string? AttendanceCode { get; set; }

    public string? EmployeeCode { get; set; }

    public string? EmployeeName { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public DateTime? CheckInTime { get; set; }

    public string? CheckInLocation { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? CheckOutLocation { get; set; }

    public decimal? WorkingHour { get; set; }

    public string? Remark { get; set; }

    public string? Status { get; set; }

    public bool? IsSavedLocation { get; set; }

    public static AttendanceListModel FromTblAttendance(TblAttendance attendance)
    {
        var model = new AttendanceListModel()
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

        // Set Status
        if (attendance.HourLateFlag == 1)
            model.Status = "Late Arrival";
        else if (attendance.HourLateFlag == 2)
            model.Status = "Early Departure";
        else if (attendance.HourLateFlag == 3)
            model.Status = "Late Arrival & Early Departure";

        else if (attendance.HalfDayFlag == 1)
            model.Status = "Half Day Late Arrival";
        else if (attendance.HalfDayFlag == 2)
            model.Status = "Half Day Early Departure";

        else if (attendance.FullDayFlag == 1)
            model.Status = "Full Day Late";

        else if (attendance.HourLateFlag == 0 && attendance.HalfDayFlag == 0 && attendance.FullDayFlag == 0)
            model.Status = "On Time";

        else if (attendance.HourLateFlag == 1 & attendance.HalfDayFlag == 2)
            model.Status = "Late Arrival Hourly & Half Day Early Departure";

        else if (attendance.HourLateFlag == 2 & attendance.HalfDayFlag == 1)
            model.Status = "Half Day Late Arrival & Early Departure Hourly";
        return model;

    }
}