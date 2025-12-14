namespace HRSystem.Csharp.Domain.Features.EmployeeAttendance;

public class DA_EmployeeAttendance : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly ILogger<DA_EmployeeAttendance> _logger;
    private readonly DA_Employee _daEmployee;

    public DA_EmployeeAttendance(IHttpContextAccessor contextAccessor,
                                 AppDbContext db,
                                 ILogger<DA_EmployeeAttendance> logger,
                                 DA_Employee daEmployee) : base(contextAccessor)
    {
        _db = db;
        _logger = logger;
        _daEmployee = daEmployee;
    }

    public async Task<bool> CheckOfficeLocation(string employeeCode, string latitude, string longitude)
    {
        if (!double.TryParse(latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat1) ||
            !double.TryParse(longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon1))
        {
            return false;
        }

        try
        {
            var employee = await _db.TblEmployees
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            if (employee == null)
            {
                return false;
            }

            var locationList = await _db.TblLocations
                .Where(x => x.DeleteFlag == false)
                .ToListAsync();

            if (!locationList.Any())
            {
                return false;
            }

            foreach (var location in locationList)
            {
                if (double.TryParse(location.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat2) &&
                    double.TryParse(location.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon2) &&
                    double.TryParse(location.Radius, NumberStyles.Any, CultureInfo.InvariantCulture, out double radius))
                {
                    var distance = CalculateDistance(lat1, lon1, lat2, lon2);

                    if (distance <= radius)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking office location for employee {EmployeeCode}", employeeCode);
            return false;
        }
    }

    public async Task<EmployeeAttendanceResponseModel?> GetAttendanceForToday(string employeeCode)
    {
        if (string.IsNullOrWhiteSpace(employeeCode))
            return null;

        var employee = await _daEmployee.GetEmployeeByCode(employeeCode);
        if (!employee.IsSuccess)
            return null;

        DateTime todayLocal = DateTime.UtcNow
                .ToLocalTime()
                .Date;

        var attendance = await _db.TblAttendances
            .AsNoTracking()
            .FirstOrDefaultAsync(a =>
                a.EmployeeCode == employeeCode &&
                a.AttendanceDate == todayLocal);

        return new EmployeeAttendanceResponseModel
        {
            AttendanceCode = attendance.AttendanceCode,
            AttendanceDate = todayLocal,
            IsCheckIn = attendance?.CheckInTime.HasValue ?? false,
            IsCheckOut = attendance?.CheckOutTime.HasValue ?? false,
            CheckInTime = attendance?.CheckInTime?.ToLocalTime().ToString("HH:mm") ?? "",
            CheckOutTime = attendance?.CheckOutTime?.ToLocalTime().ToString("HH:mm") ?? ""
        };
    }
    
    public async Task<Result<TblAttendance>> GetCheckInTime(string employeeCode)
    {
        try
        {
            var attendanceRecord = await _db.TblAttendances
                .Where(a => a.EmployeeCode == employeeCode)
                .OrderByDescending(a => a.CheckInTime)
                .FirstOrDefaultAsync();
            if (attendanceRecord is null)
            {
                return Result<TblAttendance>.NotFoundError("No attendance record found.");
            }

            return Result<TblAttendance>.Success(attendanceRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving check-in time for employee {EmployeeCode}", employeeCode);
            return Result<TblAttendance>.SystemError("An error occurred while retrieving check-in time.");
        }
    }

    public async Task<bool> CreateAttendance(AttendanceRequestModel requestModel)
    {
        var attendance = new TblAttendance()
        {
            AttendanceId = Guid.NewGuid().ToString(),
            AttendanceCode = requestModel.AttendanceCode,
            EmployeeCode = requestModel.EmployeeCode,
            AttendanceDate = DateTime.UtcNow.Date,
            CheckInTime = requestModel.CheckInTime,
            CheckInLocation = requestModel.CheckInLocation,
            CheckOutTime = requestModel.CheckOutTime,
            CheckOutLocation = requestModel.CheckOutLocation,
            WorkingHour = requestModel.WorkingHours.HasValue ? (decimal)requestModel.WorkingHours.Value.TotalHours : 0,
            HourLateFlag = requestModel.HourLateFlag,
            HalfDayFlag = requestModel.HalfDayFlag,
            FullDayFlag = requestModel.FullDayFlag,
            Remark = requestModel.Remark,
            IsCheckInLocationSaved = requestModel.IsSavedLocation,
            CreatedBy = UserCode,
            CreatedAt = DateTime.UtcNow,
            DeleteFlag = false
        };

        await _db.TblAttendances.AddAsync(attendance);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAttendance(AttendanceRequestModel requestModel)
    {
        var checkInRecord = await _db.TblAttendances
                .FirstOrDefaultAsync(a =>
                    a.AttendanceCode == requestModel.AttendanceCode &&
                    a.EmployeeCode == requestModel.EmployeeCode &&
                    a.DeleteFlag == false
                );
        if (checkInRecord is null)
        {
            return false;
        }

        checkInRecord.CheckOutTime = requestModel.CheckOutTime;
        checkInRecord.CheckOutLocation = requestModel.CheckOutLocation;
        checkInRecord.WorkingHour = requestModel.WorkingHours.HasValue ? (decimal)requestModel.WorkingHours.Value.TotalHours : 0;
        checkInRecord.HourLateFlag = requestModel.HourLateFlag;
        checkInRecord.HalfDayFlag = requestModel.HalfDayFlag;
        checkInRecord.FullDayFlag = requestModel.FullDayFlag;
        checkInRecord.Remark = requestModel.Remark;
        checkInRecord.IsCheckOutLocationSaved = requestModel.IsSavedLocation;
        checkInRecord.ModifiedBy = UserCode;
        checkInRecord.ModifiedAt = DateTime.UtcNow;

        _db.TblAttendances.Update(checkInRecord);
        await _db.SaveChangesAsync();

        return true;
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000;

        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }
}