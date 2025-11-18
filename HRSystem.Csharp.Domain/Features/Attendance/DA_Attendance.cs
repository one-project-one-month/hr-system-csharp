using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Domain.Models.Attendance;
using HRSystem.Csharp.Shared.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace HRSystem.Csharp.Domain.Features.Attendance;

public class DA_Attendance
{
    private readonly DA_Sequence _daSequence;
    private readonly AppDbContext _db;

    public DA_Attendance(AppDbContext appDbContext, DA_Sequence daSequence)
    {
        _db = appDbContext;
        _daSequence = daSequence;
    }

    public async Task<Result<AttendanceListResponseModel>> List(String? EmpName, DateTime startDate, DateTime endDate,int pageNo, int PageSize)
    {
        try
        {
            var attQuery = _db.TblAttendances.Where(x => x.DeleteFlag == false);

            if (!string.IsNullOrWhiteSpace(EmpName))
            {
                var employees = _db.TblEmployees
                            .Where(e => e.Name.Contains(EmpName) && e.DeleteFlag == false)
                            .Select(e => e.EmployeeCode);

                attQuery = attQuery.Where(x => employees.Contains(x.EmployeeCode));

            }


            if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
            {
                attQuery = attQuery.Where(x=> DateOnly.FromDateTime(x.AttendanceDate.Value) == DateOnly.FromDateTime(startDate));
            }
            else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
            {
                attQuery = attQuery.Where(x => DateOnly.FromDateTime(x.AttendanceDate.Value) == DateOnly.FromDateTime(endDate));
            }
            else if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                attQuery = attQuery.Where(x => DateOnly.FromDateTime(x.AttendanceDate.Value) >= DateOnly.FromDateTime(startDate) && DateOnly.FromDateTime(x.AttendanceDate.Value) <= DateOnly.FromDateTime(endDate));
            }
            var attendanceList = await attQuery
                    .OrderByDescending(x => x.AttendanceDate)
                    .Skip((pageNo - 1) * PageSize)
                    .Take(PageSize)
                    .AsNoTracking()
                    .ToListAsync();

            if (!attendanceList.Any() || attendanceList is null)
                return Result<AttendanceListResponseModel>.NotFoundError("No attendance found.");

            var model = new AttendanceListResponseModel
            {
                AttendanceList = attendanceList
                    .Select(t =>
                    {
                        var attendance = AttendanceListModel.FromTblAttendance(t);
                        attendance.EmployeeName = _db.TblEmployees
                            .Where(e => e.EmployeeCode == t.EmployeeCode && e.DeleteFlag == false)
                            .Select(e => e.Name)
                            .FirstOrDefault();
                        return attendance;
                    })
                    .ToList()
            };

            return Result<AttendanceListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<AttendanceListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AttendanceCreateResponseModel>> Create(AttendanceCreateRequestModel requestModel)
    {
        if (requestModel.EmployeeCode.IsNullOrEmpty())
        {
            return Result<AttendanceCreateResponseModel>.ValidationError("Employee Code is required!");
        }

        if (!requestModel.CheckInTime.HasValue)
        {
            return Result<AttendanceCreateResponseModel>.ValidationError("CheckIn Time is required!");
        }

        if (requestModel.CheckInLocation.IsNullOrEmpty())
        {
            return Result<AttendanceCreateResponseModel>.ValidationError("CheckIn Location is required!");
        }

        try
        {
            var attendanceCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.ATT.ToString());

            //Working Hour
            DateTime checkIn = (DateTime)requestModel.CheckInTime;
            DateTime checkOut = (DateTime)requestModel.CheckOutTime;
            TimeSpan workingHours = CalculateWorkingHours(checkIn, checkOut);

            //Hourly Late
            int HourLateFlag = CalculateHourlyLate(checkIn, checkOut);

            //Half Day late
            int HalfDayFlag = CalculateHalfDayLate(checkIn, checkOut);

            //Full Day late
            int FullDayFlag = 0;
            if (HalfDayFlag == 3)
            {
                FullDayFlag = 1;
            }

            //Check Location
            bool IsSavedLocation = false;
            var location = await _db.TblLocations
                .FirstOrDefaultAsync(x => x.LocationCode == requestModel.CheckInLocation
                && x.DeleteFlag == false);

            if (location != null)
            {
                IsSavedLocation = true;
            }

            var newAttendance = new TblAttendance()
            {
                AttendanceId = Guid.NewGuid().ToString(),
                AttendanceCode = attendanceCode,
                EmployeeCode = requestModel.EmployeeCode,
                AttendanceDate = DateTime.UtcNow,
                CheckInTime = requestModel.CheckInTime,
                CheckInLocation = requestModel.CheckInLocation,
                CheckOutTime = requestModel.CheckOutTime,
                CheckOutLocation = requestModel.CheckOutLocation,
                WorkingHour = (decimal)workingHours.TotalHours,
                HourLateFlag = HourLateFlag,
                HalfDayFlag = HalfDayFlag,
                FullDayFlag = FullDayFlag,
                Remark = requestModel.Remark,
                IsSavedLocation = IsSavedLocation,
                CreatedBy = null,
                CreatedAt = DateTime.UtcNow,
                DeleteFlag = false
            };
            await _db.AddAsync(newAttendance);
            await _db.SaveChangesAsync();

            return Result<AttendanceCreateResponseModel>.Success(null, "Attendance is successfully created");
        }
        catch (Exception ex)
        {
            return Result<AttendanceCreateResponseModel>.SystemError(ex.Message);
        }
    }

    public TimeSpan CalculateWorkingHours(DateTime checkIn, DateTime checkOut)
    {
        var StartTimeValue = "";
        TimeSpan officeStartTime = new System.TimeSpan();
        var OfficeEndValue = "";
        TimeSpan officeEndTime = new System.TimeSpan();

        #region Office Start Time

        var ComRuleOfficeStart = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "OFFICE_START_TIME" && x.DeleteFlag == false);
        if (ComRuleOfficeStart != null)
        {
            StartTimeValue = ComRuleOfficeStart.Value;
            if (StartTimeValue.Contains(":"))
            {
                officeStartTime = TimeSpan.Parse(StartTimeValue);
            }
            else
            {
                officeStartTime = TimeSpan.FromHours(int.Parse(StartTimeValue));
            }

        }

        #endregion

        DateTime officeStart = checkIn.Date.Add(officeStartTime);

        #region Office End Time

        var ComRuleOfficeEnd = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "OFFICE_END_TIME" && x.DeleteFlag == false);
        if (ComRuleOfficeEnd != null)
        {
            OfficeEndValue = ComRuleOfficeEnd.Value;
            if (OfficeEndValue.Contains(":"))
            {
                officeEndTime = TimeSpan.Parse(OfficeEndValue);
            }
            else
            {
                officeEndTime = TimeSpan.FromHours(int.Parse(OfficeEndValue));
            }

        }

        #endregion

        DateTime officeEnd = checkIn.Date.Add(officeEndTime);

        if (checkIn < officeStart)
            checkIn = officeStart;
        if (checkOut > officeEnd)
            checkOut = officeEnd;

        if (checkOut < checkIn)
            return TimeSpan.Zero;

        return checkOut - checkIn;
    }

    public int CalculateHourlyLate(DateTime checkIn, DateTime checkOut)
    {
        var StartTimeValue = "";
        TimeSpan StartTime = new System.TimeSpan();
        var CheckInAcceptValue = "";
        TimeSpan CheckInAccept = new System.TimeSpan();
        var CheckInLateValue = "";
        TimeSpan CheckInLate = new System.TimeSpan();
        var OfficeEndValue = "";
        TimeSpan OfficeEnd = new System.TimeSpan();
        var CheckoutAcceptValue = "";
        TimeSpan CheckoutAccept = new System.TimeSpan();
        var CheckoutLateValue = "";
        TimeSpan CheckoutLate = new System.TimeSpan();

        #region Office Start Time

        var ComRuleOfficeStart = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "OFFICE_START_TIME" && x.DeleteFlag == false);
        if (ComRuleOfficeStart != null)
        {
            StartTimeValue = ComRuleOfficeStart.Value;
            if (StartTimeValue.Contains(":"))
            {
                StartTime = TimeSpan.Parse(StartTimeValue);
            }
            else
            {
                StartTime = TimeSpan.FromHours(int.Parse(StartTimeValue));
            }

        }

        #endregion

        DateTime officeStart = checkIn.Date.Add(StartTime);

        #region Office Acceptable CheckIn

        var ComRuleCheckinAccept = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "CHECKIN_ACCEPTABLE" && x.DeleteFlag == false);
        if (ComRuleCheckinAccept != null)
        {
            CheckInAcceptValue = ComRuleCheckinAccept.Value;
            if (CheckInAcceptValue.Contains(":"))
            {
                CheckInAccept = TimeSpan.Parse(CheckInAcceptValue);
            }
            else
            {
                CheckInAccept = TimeSpan.FromHours(int.Parse(CheckInAcceptValue));
            }

        }

        #endregion

        DateTime MorningFirstLate = checkIn.Date.Add(CheckInAccept);

        #region One Hour Late CheckIn

        var ComRuleCheckinLate = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "CHECKIN_ONE_HOUR_LATE" && x.DeleteFlag == false);
        if (ComRuleCheckinLate != null)
        {
            CheckInLateValue = ComRuleCheckinLate.Value;
            if (CheckInLateValue.Contains(":"))
            {
                CheckInLate = TimeSpan.Parse(CheckInLateValue);
            }
            else
            {
                CheckInLate = TimeSpan.FromHours(int.Parse(CheckInLateValue));
            }
        }

        #endregion

        DateTime MorningSecondLate = checkIn.Date.Add(CheckInLate);

        int hourLate = 0;
        if (checkIn > MorningFirstLate && checkIn <= MorningSecondLate)
            hourLate = 1;


        //For CheckOut Late
        #region Office End Time

        var ComRuleOfficeEnd = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "OFFICE_END_TIME" && x.DeleteFlag == false);
        if (ComRuleOfficeEnd != null)
        {
            OfficeEndValue = ComRuleOfficeEnd.Value;
            if (OfficeEndValue.Contains(":"))
            {
                OfficeEnd = TimeSpan.Parse(OfficeEndValue);
            }
            else
            {
                OfficeEnd = TimeSpan.FromHours(int.Parse(OfficeEndValue));
            }
        }

        #endregion

        DateTime officeEnd = checkIn.Date.Add(OfficeEnd);

        #region Office Acceptable Checkout

        var ComRuleCheckoutAccept = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "CHECKOUT_ACCEPTABLE" && x.DeleteFlag == false);
        if (ComRuleCheckoutAccept != null)
        {
            CheckoutAcceptValue = ComRuleCheckoutAccept.Value;
            if (CheckoutAcceptValue.Contains(":"))
            {
                CheckoutAccept = TimeSpan.Parse(CheckoutAcceptValue);
            }
            else
            {
                CheckoutAccept = TimeSpan.FromHours(int.Parse(CheckoutAcceptValue));
            }
        }

        #endregion

        DateTime eveningFirstLate = checkIn.Date.Add(CheckoutAccept);

        #region One Hour Late Checkout 

        var ComRuleCheckoutLate = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "CHECKOUT_HOURLATE" && x.DeleteFlag == false);
        if (ComRuleCheckoutLate != null)
        {
            CheckoutLateValue = ComRuleCheckoutLate.Value;
            if (CheckoutLateValue.Contains(":"))
            {
                CheckoutLate = TimeSpan.Parse(CheckoutLateValue);
            }
            else
            {
                CheckoutLate = TimeSpan.FromHours(int.Parse(CheckoutLateValue));
            }
        }

        #endregion

        DateTime eveningSecondLate = checkIn.Date.Add(CheckoutLate);

        if (checkOut < eveningFirstLate && checkOut >= eveningSecondLate)
            hourLate = 2;  // Can assume Evening Late for 2

        if (checkIn > MorningFirstLate && checkIn <= MorningSecondLate && checkOut < eveningFirstLate && checkOut >= eveningSecondLate)
            hourLate = 3;  // both late

        return hourLate;
    }

    public int CalculateHalfDayLate(DateTime checkIn, DateTime checkOut)
    {
        int halfDayLate = 0;
        var CheckInLateValue = "";
        TimeSpan CheckInLate = new System.TimeSpan();
        var CheckoutLateValue = "";
        TimeSpan CheckoutLate = new System.TimeSpan();

        //For Morning Part
        #region One Hour Late CheckIn

        var ComRuleCheckinLate = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "CHECKIN_ONE_HOUR_LATE" && x.DeleteFlag == false);
        if (ComRuleCheckinLate != null)
        {
            CheckInLateValue = ComRuleCheckinLate.Value;
            if (CheckInLateValue.Contains(":"))
            {
                CheckInLate = TimeSpan.Parse(CheckInLateValue);
            }
            else
            {
                CheckInLate = TimeSpan.FromHours(int.Parse(CheckInLateValue));
            }
        }

        #endregion

        DateTime MorningLate = checkIn.Date.Add(CheckInLate);
        if (checkIn > MorningLate)
            halfDayLate = 1;

        //For Evening Part
        #region One Hour Late Checkout

        var ComRuleCheckoutLate = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "CHECKOUT_HOURLATE" && x.DeleteFlag == false);
        if (ComRuleCheckoutLate != null)
        {
            CheckoutLateValue = ComRuleCheckoutLate.Value;
            if (CheckoutLateValue.Contains(":"))
            {
                CheckoutLate = TimeSpan.Parse(CheckoutLateValue);
            }
            else
            {
                CheckoutLate = TimeSpan.FromHours(int.Parse(CheckoutLateValue));
            }
        }

        #endregion

        DateTime eveningLate = checkIn.Date.Add(CheckoutLate);

        if (checkOut < eveningLate)
            halfDayLate = 2;


        if (checkIn > MorningLate && checkOut < eveningLate)
            halfDayLate = 3;

        return halfDayLate;
    }

    public async Task<Result<AttendanceUpdateResponseModel>> Update(AttendanceUpdateRequestModel requestModel)
    {
        try
        {
            var item = await _db.TblAttendances
           .FirstOrDefaultAsync(
           x => x.AttendanceCode == requestModel.AttendanceCode
            && x.DeleteFlag == false);

            if (item is null)
            {
                return Result<AttendanceUpdateResponseModel>.NotFoundError("Attendance not found!");
            }

            //Working Hour
            DateTime checkIn = (DateTime)requestModel.CheckInTime;
            DateTime checkOut = (DateTime)requestModel.CheckOutTime;
            TimeSpan workingHours = CalculateWorkingHours(checkIn, checkOut);

            //Hourly Late
            int HourLateFlag = CalculateHourlyLate(checkIn, checkOut);

            //Half Day late
            int HalfDayFlag = CalculateHalfDayLate(checkIn, checkOut);

            //Full Day late
            int FullDayFlag = 0;
            if (HalfDayFlag == 3)
            {
                FullDayFlag = 1;
            }

            //Check Location
            bool IsSavedLocation = false;
            var location = await _db.TblLocations
                .FirstOrDefaultAsync(x => x.LocationCode == requestModel.CheckInLocation
                && x.DeleteFlag == false);

            if (location != null)
            {
                IsSavedLocation = true;
            }

            item.EmployeeCode = requestModel.EmployeeCode;
            item.AttendanceDate = requestModel.AttendanceDate;
            item.CheckInTime = requestModel.CheckInTime;
            item.CheckInLocation = requestModel.CheckInLocation;
            item.CheckOutTime = requestModel.CheckOutTime;
            item.CheckOutLocation = requestModel.CheckOutLocation;
            item.WorkingHour = (decimal)workingHours.TotalHours;
            item.HourLateFlag = HourLateFlag;
            item.HalfDayFlag = HalfDayFlag;
            item.FullDayFlag = FullDayFlag;
            item.IsSavedLocation = IsSavedLocation;
            item.Remark = requestModel.Remark;
            item.ModifiedBy = "";
            item.ModifiedAt = DateTime.UtcNow;
            _db.Entry(item).State = EntityState.Modified;
            var res = await _db.SaveChangesAsync();
            foreach (var entry in _db.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }

            return Result<AttendanceUpdateResponseModel>.Success(null, "Attendance updated successfully!");
        }
        catch (Exception ex)
        {
            return Result<AttendanceUpdateResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AttendanceEditResponseModel>> Edit(string attendanceCode)
    {
        var model = new AttendanceEditResponseModel();
        if (attendanceCode.IsNullOrEmpty())
        {
            return Result<AttendanceEditResponseModel>.ValidationError("Attendance Code required.");
        }
        try
        {
            var item = await _db.TblAttendances
            .FirstOrDefaultAsync(x => x.AttendanceCode == attendanceCode
            && x.DeleteFlag == false);

            if (item is null)
            {
                return Result<AttendanceEditResponseModel>.NotFoundError("Attendance not found.");
            }

            model.Attendance = AttendanceEditModel.FromTblAttendance(item);
            return Result<AttendanceEditResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<AttendanceEditResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AttendanceDeleteResponseModel>> Delete(string attendanceCode)
    {
        var model = new AttendanceEditResponseModel();
        if (attendanceCode.IsNullOrEmpty())
        {
            return Result<AttendanceDeleteResponseModel>.ValidationError("Attendance Code required.");
        }
        try
        {
            var item = await _db.TblAttendances
            .FirstOrDefaultAsync(x => x.AttendanceCode == attendanceCode
            && x.DeleteFlag == false);

            if (item is null)
            {
                return Result<AttendanceDeleteResponseModel>.NotFoundError("Attendance not found.");
            }

            item.DeleteFlag = true;
            item.ModifiedAt = DateTime.UtcNow;

            _db.Entry(item).State = EntityState.Modified;
            var res = await _db.SaveChangesAsync();
            foreach (var entry in _db.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }

            return Result<AttendanceDeleteResponseModel>.Success(null, "Attendance successfully deleted!");

        }
        catch (Exception ex)
        {
            return Result<AttendanceDeleteResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<string> GenerateSequenceCodeAsync(string uniqueName)
    {
        var sequence = await _db.TblSequences
            .FirstOrDefaultAsync(s => s.UniqueName == uniqueName);

        if (sequence is null)
        {
            throw new Exception("Sequence not found.");
        }

        var sequenceNo = Int32.Parse(sequence.SequenceNo!) + 1;

        var sequenceCode = uniqueName + sequenceNo.ToString("D6");
        return sequenceCode;
    }

    public void UpdateSequenceNoAsync(string uniqueName, string sequenceNo)
    {
        var sequence = _db.TblSequences
            .FirstOrDefault(s => s.UniqueName == uniqueName);

        if (sequence is null)
        {
            throw new Exception("Sequence not found.");
        }

        sequence.SequenceNo = sequenceNo;

        _db.Entry(sequence).State = EntityState.Modified;
        _db.SaveChanges();
    }
}