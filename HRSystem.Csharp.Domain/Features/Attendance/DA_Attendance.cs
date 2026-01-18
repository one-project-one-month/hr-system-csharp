using HRSystem.Csharp.Domain.Features.CompanyRule;
using HRSystem.Csharp.Domain.Models.Attendance;

namespace HRSystem.Csharp.Domain.Features.Attendance;

public class DA_Attendance(AppDbContext appDbContext, DA_Sequence daSequence, DA_CompanyRule _daCompanyRule)
{
    private readonly DA_Sequence _daSequence = daSequence;
    private readonly AppDbContext _db = appDbContext;
    private readonly DA_CompanyRule _daCompanyRule = _daCompanyRule;

    public async Task<Result<AttendanceListResponseModel>> List(String? EmpName, DateTime startDate, DateTime endDate,
        int pageNo, int PageSize)
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
                attQuery = attQuery.Where(x =>
                    DateOnly.FromDateTime(x.AttendanceDate!.Value) == DateOnly.FromDateTime(startDate));
            }
            else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
            {
                attQuery = attQuery.Where(x =>
                    DateOnly.FromDateTime(x.AttendanceDate!.Value) == DateOnly.FromDateTime(endDate));
            }
            else if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                attQuery = attQuery.Where(x =>
                    DateOnly.FromDateTime(x.AttendanceDate!.Value) >= DateOnly.FromDateTime(startDate) &&
                    DateOnly.FromDateTime(x.AttendanceDate.Value) <= DateOnly.FromDateTime(endDate));
            }

            var attendanceList = await attQuery
                .OrderByDescending(x => x.AttendanceDate)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .AsNoTracking()
                .ToListAsync();

            if (attendanceList.Count == 0 || attendanceList is null)
                return Result<AttendanceListResponseModel>.NotFoundError("No attendance found.");

            var model = new AttendanceListResponseModel
            {
                AttendanceList =  [.. attendanceList
                .Select(t =>
                {
                var attendance = AttendanceListModel.FromTblAttendance(t);
                attendance.EmployeeName = _db.TblEmployees
                .Where(e => e.EmployeeCode == t.EmployeeCode && e.DeleteFlag == false)
                .Select(e => e.Name)
                .FirstOrDefault();
                return attendance;
            })]
            };

            return Result<AttendanceListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<AttendanceListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AttendanceListResponseModel>> ListByCode(String? empCode, DateTime startDate,
        DateTime endDate, int pageNo, int PageSize)
    {
        try
        {
            var attQuery = _db.TblAttendances.Where(x => x.DeleteFlag == false);

            if (!string.IsNullOrWhiteSpace(empCode))
            {
                attQuery = attQuery.Where(x => x.EmployeeCode == empCode);
            }

            if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
            {
                attQuery = attQuery.Where(x =>
                    DateOnly.FromDateTime(x.AttendanceDate!.Value) == DateOnly.FromDateTime(startDate));
            }
            else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
            {
                attQuery = attQuery.Where(x =>
                    DateOnly.FromDateTime(x.AttendanceDate!.Value) == DateOnly.FromDateTime(endDate));
            }
            else if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                attQuery = attQuery.Where(x =>
                    DateOnly.FromDateTime(x.AttendanceDate!.Value) >= DateOnly.FromDateTime(startDate) &&
                    DateOnly.FromDateTime(x.AttendanceDate.Value) <= DateOnly.FromDateTime(endDate));
            }

            var attendanceList = await attQuery
                .OrderByDescending(x => x.AttendanceDate)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .AsNoTracking()
                .ToListAsync();

            if (attendanceList.Count == 0 || attendanceList is null)
                return Result<AttendanceListResponseModel>.NotFoundError("No attendance found.");

            var model = new AttendanceListResponseModel
            {
                AttendanceList =  [.. attendanceList
                .Select(t =>
                {
                var attendance = AttendanceListModel.FromTblAttendance(t);
                attendance.EmployeeName = _db.TblEmployees
                .Where(e => e.EmployeeCode == t.EmployeeCode && e.DeleteFlag == false)
                .Select(e => e.Name)
                .FirstOrDefault();
                return attendance;
            })]
            };

            return Result<AttendanceListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<AttendanceListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AttendanceCreateResponseModel>> Create(string userId,
        AttendanceCreateRequestModel requestModel)
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
            TimeSpan workingHours = await CalculateWorkingHours(checkIn, checkOut);

            //Hourly Late
            int HourLateFlag = await CalculateHourlyLate(checkIn, checkOut);

            //Half Day late
            int HalfDayFlag = await CalculateHalfDayLate(checkIn, checkOut);

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
                IsCheckInLocationSaved = IsSavedLocation,
                IsCheckOutLocationSaved = IsSavedLocation,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                DeleteFlag = false
            };
            await _db.AddAsync(newAttendance);
            var res = await _db.SaveChangesAsync();
            foreach (var entry in _db.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }

            return Result<AttendanceCreateResponseModel>.Success("Attendance is successfully created");
        }
        catch (Exception ex)
        {
            return Result<AttendanceCreateResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AttendanceUpdateResponseModel>> Update(string userId,
        AttendanceUpdateRequestModel requestModel)
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
            DateTime checkIn = (DateTime)requestModel.CheckInTime!;
            DateTime checkOut = (DateTime)requestModel.CheckOutTime!;
            TimeSpan workingHours = await CalculateWorkingHours(checkIn, checkOut);

            //Hourly Late
            int HourLateFlag = await CalculateHourlyLate(checkIn, checkOut);

            //Half Day late
            int HalfDayFlag = await CalculateHalfDayLate(checkIn, checkOut);

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
            item.IsCheckInLocationSaved = IsSavedLocation;
            item.IsCheckOutLocationSaved = IsSavedLocation;
            item.Remark = requestModel.Remark;
            item.ModifiedBy = userId;
            item.ModifiedAt = DateTime.UtcNow;
            _db.Entry(item).State = EntityState.Modified;
            var res = await _db.SaveChangesAsync();
            foreach (var entry in _db.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }

            return Result<AttendanceUpdateResponseModel>.Success("Attendance updated successfully!");
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

            return Result<AttendanceDeleteResponseModel>.Success("Attendance successfully deleted!");
        }
        catch (Exception ex)
        {
            return Result<AttendanceDeleteResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<TimeSpan> CalculateWorkingHours(DateTime checkIn, DateTime checkOut)
    {
        // Old approach - commented out
        /*
        var StartTimeValue = "";
        TimeSpan officeStartTime = new();
        var OfficeEndValue = "";
        TimeSpan officeEndTime = new();

        #region Office Start Time

        var ComRuleOfficeStart = _db.TblCompanyRules.FirstOrDefault(x => x.CompanyRuleCode == "OFFICE_START_TIME" && x.DeleteFlag == false);
        if (ComRuleOfficeStart != null)
        {
            StartTimeValue = ComRuleOfficeStart.Value;
            if (StartTimeValue!.Contains(':'))
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
            if (OfficeEndValue!.Contains(':'))
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
        */

        // New approach using DA_CompanyRule
        var officeStartTimeValue =
            await _daCompanyRule.GetRuleValue(EnumRuleCode.OfficeHourStartTime.ToEnumDescription());
        var officeEndTimeValue = await _daCompanyRule.GetRuleValue(EnumRuleCode.OfficeHourEndTime.ToEnumDescription());

        TimeSpan officeStartTime = TimeSpan.FromHours(officeStartTimeValue);
        TimeSpan officeEndTime = TimeSpan.FromHours(officeEndTimeValue);

        DateTime officeStart = checkIn.Date.Add(officeStartTime);
        DateTime officeEnd = checkIn.Date.Add(officeEndTime);

        if (checkIn < officeStart)
        {
            checkIn = officeStart;
        }

        if (checkOut > officeEnd)
        {
            checkOut = officeEnd;
        }

        if (checkOut < checkIn)
        {
            return TimeSpan.Zero;
        }

        return checkOut - checkIn;
    }

    public async Task<int> CalculateHourlyLate(DateTime checkIn, DateTime checkOut)
    {
        // Get all rule values using DA_CompanyRule
        var officeStartTimeValue = await _daCompanyRule.GetRuleValue(EnumRuleCode.OfficeHourStartTime.ToEnumDescription());
        var checkInAcceptableValue = await _daCompanyRule.GetRuleTimeValue(EnumRuleCode.CheckInAcceptable.ToEnumDescription());
        var checkInOneHourLateValue = await _daCompanyRule.GetRuleValue(EnumRuleCode.CheckInOneHourLate.ToEnumDescription());
        
        var officeEndTimeValue = await _daCompanyRule.GetRuleValue(EnumRuleCode.OfficeHourEndTime.ToEnumDescription());
        var checkOutAcceptableValue = await _daCompanyRule.GetRuleTimeValue(EnumRuleCode.CheckOutAcceptable.ToEnumDescription());
        var checkOutOneHourLateValue = await _daCompanyRule.GetRuleValue(EnumRuleCode.CheckOutOneHourLate.ToEnumDescription());

        TimeSpan officeStartTime = TimeSpan.FromHours(officeStartTimeValue);
        TimeSpan checkInAcceptable = checkInAcceptableValue;
        TimeSpan checkInOneHourLate = TimeSpan.FromHours(checkInOneHourLateValue);
        
        TimeSpan officeEndTime = TimeSpan.FromHours(officeEndTimeValue);
        TimeSpan checkOutAcceptable = checkOutAcceptableValue;
        TimeSpan checkOutOneHourLate = TimeSpan.FromHours(checkOutOneHourLateValue);

        DateTime officeStart = checkIn.Date.Add(officeStartTime);
        DateTime checkInAcceptableTime = checkIn.Date.Add(checkInAcceptable);
        DateTime checkInOneHourLateTime = checkIn.Date.Add(checkInOneHourLate);
        
        DateTime officeEnd = checkIn.Date.Add(officeEndTime);
        DateTime checkOutAcceptableTime = checkIn.Date.Add(checkOutAcceptable);
        DateTime checkOutOneHourLateTime = checkIn.Date.Add(checkOutOneHourLate);

        int hourLate = 0;

        // Check-in logic: when check in time is between office start time and check in acceptable time (9:30), it's ok
        // When check in time is between CheckInAcceptable (9:30) and CheckInOneHourLate, set hour late flag to 1
        if (checkIn > checkInAcceptableTime && checkIn <= checkInOneHourLateTime)
        {
            hourLate = 1; // Check-in hour late
        }

        // Check-out logic: when check out time is before office end time and between office end time and check out acceptable (16:30), it's ok
        // When check out time is between check out acceptable (16:30) and check out one hour late, set hour late flag to 1
        if (checkOut < checkOutAcceptableTime && checkOut >= checkOutOneHourLateTime)
        {
            hourLate += 1; // Add 1 for check-out hour late
        }

        return hourLate;
    }

    public async Task<int> CalculateHalfDayLate(DateTime checkIn, DateTime checkOut)
    {
        // Get all rule values using DA_CompanyRule
        var checkInOneHourLateValue = await _daCompanyRule.GetRuleValue(EnumRuleCode.CheckInOneHourLate.ToEnumDescription());
        var checkOutOneHourLateValue = await _daCompanyRule.GetRuleValue(EnumRuleCode.CheckOutOneHourLate.ToEnumDescription());

        TimeSpan checkInOneHourLate = TimeSpan.FromHours(checkInOneHourLateValue);
        TimeSpan checkOutOneHourLate = TimeSpan.FromHours(checkOutOneHourLateValue);

        DateTime checkInOneHourLateTime = checkIn.Date.Add(checkInOneHourLate);
        DateTime checkOutOneHourLateTime = checkIn.Date.Add(checkOutOneHourLate);

        int halfDayLate = 0;

        // When check in time is over CheckInOneHourLate, set half day late to 1
        if (checkIn > checkInOneHourLateTime)
        {
            halfDayLate = 1; // Check-in half day late
        }

        // When check out time is before check out one hour late, set half day late to 1
        // If half day late is already 1 (from check in), increase it by 1 to make it 2
        if (checkOut < checkOutOneHourLateTime)
        {
            halfDayLate += 1; // Add 1 for check-out half day late
        }

        return halfDayLate;
    }

    public async Task<string> GenerateSequenceCodeAsync(string uniqueName)
    {
        var sequence = await _db.TblSequences
            .FirstOrDefaultAsync(s => s.UniqueName == uniqueName) ?? throw new Exception("Sequence not found.");
        var sequenceNo = int.Parse(sequence.SequenceNo!) + 1;

        var sequenceCode = uniqueName + sequenceNo.ToString("D6");
        return sequenceCode;
    }

    public void UpdateSequenceNoAsync(string uniqueName, string sequenceNo)
    {
        var sequence = _db.TblSequences
            .FirstOrDefault(s => s.UniqueName == uniqueName) ?? throw new Exception("Sequence not found.");
        sequence.SequenceNo = sequenceNo;

        _db.Entry(sequence).State = EntityState.Modified;
        _db.SaveChanges();
    }
}