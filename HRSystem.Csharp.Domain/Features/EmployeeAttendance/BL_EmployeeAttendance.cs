namespace HRSystem.Csharp.Domain.Features.EmployeeAttendance;

public class BL_EmployeeAttendance
{
    private readonly DA_EmployeeAttendance _da;
    private readonly DA_Sequence _daSequence;
    private readonly DA_Attendance _daAttendance;

    public BL_EmployeeAttendance(DA_EmployeeAttendance da, DA_Sequence daSequence, DA_Attendance daAttendance)
    {
        _da = da;
        _daSequence = daSequence;
        _daAttendance = daAttendance;
    }

    public async Task<Result<EmployeeAttendanceResponseModel>> AttendanceCheck(
        EmployeeAttendanceRequestModel requestModel)
    {
        #region Validation Request

        if (requestModel.EmployeeCode.IsNullOrEmpty())
        {
            return Result<EmployeeAttendanceResponseModel>.ValidationError("Employee Code is required!");
        }

        if (requestModel.CheckInStatus.IsNullOrEmpty())
        {
            return Result<EmployeeAttendanceResponseModel>.ValidationError("Check In Status is required!");
        }

        if (requestModel.Latitude.IsNullOrEmpty())
        {
            return Result<EmployeeAttendanceResponseModel>.ValidationError("Latitude is required!");
        }

        if (requestModel.Longitude.IsNullOrEmpty())
        {
            return Result<EmployeeAttendanceResponseModel>.ValidationError("Longitude is required!");
        }

        #endregion

        try
        {
            #region Generate Sequence Code

            var attendanceCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.ATT.ToString());

            #endregion

            DateTime checkIn = DateTime.UtcNow;
            DateTime? checkOut = null;
            TimeSpan? workingHours = null;
            var checkInData = new Result<TblAttendance>();
            int? hourLateFlag = null;
            int? halfDayFlag = null;
            int? fullDayFlag = null;
            bool result;

            #region Validate Working Hour and Late Hour

            if (requestModel.CheckInStatus == EnumCheckInStatus.CheckIn.ToString())
            {
                checkIn = DateTime.UtcNow;
            }
            else
            {
                checkOut = DateTime.UtcNow;
                checkInData = await _da.GetCheckInTime(requestModel.EmployeeCode!);
                checkIn = DateTime.Parse(checkInData.Data!.CheckInTime.ToString()!);

                // Working Hour
                workingHours = _daAttendance.CalculateWorkingHours(checkIn, checkOut.Value);

                // Hourly Late
                hourLateFlag = _daAttendance.CalculateHourlyLate(checkIn, checkOut.Value);

                // Half Day late
                halfDayFlag = _daAttendance.CalculateHalfDayLate(checkIn, checkOut.Value);

                // Full Day late
                if (halfDayFlag == 3)
                {
                    fullDayFlag = 1;
                }
            }

            #endregion

            #region Validate Location

            var isSavedLocation = await _da.CheckOfficeLocation(
                requestModel.EmployeeCode!,
                requestModel.Latitude!,
                requestModel.Longitude!);

            #endregion

            #region Check In Attendance

            if (requestModel.CheckInStatus == EnumCheckInStatus.CheckIn.ToString())
            {
                var checkInLocation = $"{requestModel.Latitude},{requestModel.Longitude}";

                var createModel = new AttendanceRequestModel
                {
                    AttendanceCode = attendanceCode,
                    EmployeeCode = requestModel.EmployeeCode!,
                    CheckInTime = checkIn,
                    CheckInLocation = checkInLocation,
                    CheckOutTime = null,
                    CheckOutLocation = null!,
                    WorkingHours = workingHours,
                    HourLateFlag = hourLateFlag,
                    HalfDayFlag = halfDayFlag,
                    FullDayFlag = fullDayFlag,
                    Remark = requestModel.Remark!,
                    IsSavedLocation = isSavedLocation
                };

                result = await _da.CreateAttendance(createModel);

                return result == true
                    ? Result<EmployeeAttendanceResponseModel>.Success("Check In successful.")
                    : Result<EmployeeAttendanceResponseModel>.Error("Failed to create attendance record.");
            }

            #endregion

            #region Check Out Attendance

            else
            {
                var checkOutLocation = $"{requestModel.Latitude},{requestModel.Longitude}";
                var todayAttendance = await _da.GetAttendanceForToday(requestModel.EmployeeCode!);

                if (todayAttendance is null)
                {
                    return Result<EmployeeAttendanceResponseModel>.NotFoundError("Please check in first!");
                }

                var updateModel = new AttendanceRequestModel
                {
                    AttendanceCode = todayAttendance?.AttendanceCode,
                    EmployeeCode = requestModel.EmployeeCode!,
                    CheckInTime = (DateTime)checkInData.Data!.CheckInTime!,
                    CheckInLocation = checkInData.Data.CheckInLocation!,
                    CheckOutTime = checkOut,
                    CheckOutLocation = checkOutLocation,
                    WorkingHours = workingHours,
                    HourLateFlag = hourLateFlag,
                    HalfDayFlag = halfDayFlag,
                    FullDayFlag = fullDayFlag,
                    Remark = requestModel.Remark!,
                    IsSavedLocation = isSavedLocation
                };
                result = await _da.UpdateAttendance(updateModel);

                return result == true
                    ? Result<EmployeeAttendanceResponseModel>.Success("Check Out successful.")
                    : Result<EmployeeAttendanceResponseModel>.Error("Failed to update attendance record.");
            }

            #endregion
        }
        catch (Exception ex)
        {
            return Result<EmployeeAttendanceResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<EmployeeAttendanceResponseModel>> GetAttendanceForToday(string employeeCode)
    {
        if (employeeCode is null)
            return Result<EmployeeAttendanceResponseModel>.InvalidDataError("Employee Code is required!");

        var response = await _da.GetAttendanceForToday(employeeCode);

        if (response is null)
            return Result<EmployeeAttendanceResponseModel>.BadRequestError("Employee not found");
        return Result<EmployeeAttendanceResponseModel>.Success(response);
    }
}