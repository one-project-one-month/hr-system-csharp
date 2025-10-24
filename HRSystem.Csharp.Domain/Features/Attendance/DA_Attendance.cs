using HRSystem.Csharp.Domain.Models.Attendance;
using Microsoft.IdentityModel.Tokens;

namespace HRSystem.Csharp.Domain.Features.Attendance
{
    public class DA_Attendance
    {
        private readonly AppDbContext _db;

        public DA_Attendance(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }

        public async Task<Result<AttendanceListResponseModel>> List(int pageNo, int PageSize)
        {
            try
            {
                var attendanceList = await _db.TblAttendances.Where(x => x.DeleteFlag == false)
                .OrderByDescending(x => x.AttendanceDate)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

                if (!attendanceList.Any() || attendanceList is null)
                    return Result<AttendanceListResponseModel>.NotFoundError("No attendance found.");

                var model = new AttendanceListResponseModel
                {
                    AttendanceList = attendanceList
                    .Select(AttendanceListModel.FromTblAttendance)
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
            if(requestModel.EmployeeCode.IsNullOrEmpty())
            {
                return Result<AttendanceCreateResponseModel>.ValidationError("Employee Code is required!");
            }

            if (!requestModel.AttendanceDate.HasValue)
            {
                return Result<AttendanceCreateResponseModel>.ValidationError("Attendance Date is required!");
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
                var attendanceCode = await GenerateSequenceCodeAsync("AT");
                
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
                if (HalfDayFlag == 2)
                {
                    FullDayFlag = 1;
                }

                //Check Location
                bool IsSavedLocation = false;
                var location = await _db.TblLocations
                    .FirstOrDefaultAsync(x => x.LocationCode == requestModel.CheckInLocation
                    && x.DeleteFlag == false);
                
                if(location != null)
                {
                    IsSavedLocation = true;
                }

                var newAttendance = new TblAttendance()
                {
                    AttendanceId = Guid.NewGuid().ToString(),
                    AttendanceCode = attendanceCode,
                    EmployeeCode = requestModel.EmployeeCode,
                    AttendanceDate = requestModel.AttendanceDate,
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

                UpdateSequenceNoAsync("AT", attendanceCode.Substring(2));

                return Result<AttendanceCreateResponseModel>.Success(null,"Attendance is successfully created");
            }
            catch (Exception ex)
            {
                return Result<AttendanceCreateResponseModel>.SystemError(ex.Message);
            }
        }

        public TimeSpan CalculateWorkingHours(DateTime checkIn, DateTime checkOut)
        {
            var StartTimeValue = "";
            double StartTime = 0.0;
            var OfficeEndValue = "";
            double OfficeEnd = 0.0;

            #region
            var ComRuleOfficeStart = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "OFFICE_START_TIME" && x.DeleteFlag == false);
            if (ComRuleOfficeStart != null)
            {
                StartTimeValue = ComRuleOfficeStart.Value;
                StartTime = Double.Parse(StartTimeValue);
            }
            #endregion
            DateTime officeStart = checkIn.Date.AddHours(StartTime);

            #region
            var ComRuleOfficeEnd = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "OFFICE_END_TIME" && x.DeleteFlag == false);
            if (ComRuleOfficeEnd != null)
            {
                OfficeEndValue = ComRuleOfficeEnd.Value;
                OfficeEnd = Double.Parse(OfficeEndValue);
            }
            #endregion
            DateTime officeEnd = checkIn.Date.AddHours(OfficeEnd);  

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
            double StartTime = 0.0;
            var CheckInAcceptValue = "";
            double CheckInAccept = 0.0;
            var CheckInLateValue = "";
            double CheckInLate = 0.0;
            var OfficeEndValue = "";
            double OfficeEnd = 0.0; 
            var CheckoutAcceptValue = "";
            double CheckoutAccept = 0.0;
            var CheckoutLateValue = "";
            double CheckoutLate = 0.0;

            #region
            var ComRuleOfficeStart =  _db.TblCompanyRules.FirstOrDefault(x => x.Description == "OFFICE_START_TIME" && x.DeleteFlag == false);
            if (ComRuleOfficeStart != null)
            {
                StartTimeValue = ComRuleOfficeStart.Value;
                StartTime = Double.Parse(StartTimeValue);
            }
            #endregion
            DateTime officeStart = checkIn.Date.AddHours(StartTime);

            #region
            var ComRuleCheckinAccept = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "CHECKIN_ACCEPTABLE" && x.DeleteFlag == false);
            if (ComRuleCheckinAccept != null)
            {
                CheckInAcceptValue = ComRuleCheckinAccept.Value;
                CheckInAccept = Double.Parse(CheckInAcceptValue);
            }
            #endregion
            DateTime MorningFirstLate = checkIn.Date.AddHours(CheckInAccept); 

            #region
            var ComRuleCheckinLate = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "CHECKIN_ONE_HOUR_LATE" && x.DeleteFlag == false);
            if (ComRuleCheckinLate != null)
            {
                CheckInLateValue = ComRuleCheckinLate.Value;
                CheckInLate = Double.Parse(CheckInLateValue);
            }
            #endregion
            DateTime MorningSecondLate = checkIn.Date.AddHours(CheckInLate);  

            int hourLate = 0;
            if (checkIn > MorningFirstLate && checkIn <= MorningSecondLate)
                hourLate = 1;


            //For CheckOut Late
            #region
            var ComRuleOfficeEnd = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "OFFICE_END_TIME" && x.DeleteFlag == false);
            if (ComRuleOfficeEnd != null)
            {
                OfficeEndValue = ComRuleOfficeEnd.Value;
                OfficeEnd = Double.Parse(OfficeEndValue);
            }
            #endregion
            DateTime officeEnd = checkIn.Date.AddHours(OfficeEnd);

            #region
            var ComRuleCheckoutAccept = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "CHECKOUT_ACCEPTABLE" && x.DeleteFlag == false);
            if (ComRuleCheckoutAccept != null)
            {
                CheckoutAcceptValue = ComRuleCheckoutAccept.Value;
                CheckoutAccept = Double.Parse(CheckoutAcceptValue);
            }
            #endregion
            DateTime eveningFirstLate = checkIn.Date.AddHours(CheckoutAccept);

            #region
            var ComRuleCheckoutLate = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "CHECKOUT_HOURLATE" && x.DeleteFlag == false);
            if (ComRuleCheckoutLate != null)
            {
                CheckoutLateValue = ComRuleCheckoutLate.Value;
                CheckoutLate = Double.Parse(CheckoutLateValue);
            }
            #endregion
            DateTime eveningSecondLate = checkIn.Date.AddHours(CheckoutLate);  

            if (checkOut < eveningFirstLate && checkOut >= eveningSecondLate)
                hourLate += 1;

            return hourLate;
        }

        public int CalculateHalfDayLate(DateTime checkIn, DateTime checkOut)
        {
            int halfDayLate = 0;
            var CheckInLateValue = "";
            double CheckInLate = 0.0;
            var CheckoutLateValue = "";
            double CheckoutLate = 0.0;

            //For Morning Part
            #region
            var ComRuleCheckinLate = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "CHECKIN_ONE_HOUR_LATE" && x.DeleteFlag == false);
            if (ComRuleCheckinLate != null)
            {
                CheckInLateValue = ComRuleCheckinLate.Value;
                CheckInLate = Double.Parse(CheckInLateValue);
            }
            #endregion
            DateTime MorningLate = checkIn.Date.AddHours(CheckInLate); 
            if (checkIn > MorningLate)
                halfDayLate = 1;

            //For Evening Part
            #region
            var ComRuleCheckoutLate = _db.TblCompanyRules.FirstOrDefault(x => x.Description == "CHECKOUT_HOURLATE" && x.DeleteFlag == false);
            if (ComRuleCheckoutLate != null)
            {
                CheckoutLateValue = ComRuleCheckoutLate.Value;
                CheckoutLate = Double.Parse(CheckoutLateValue);
            }
            #endregion
            DateTime eveningLate = checkIn.Date.AddHours(CheckoutLate); 

            if (checkOut < eveningLate)
                halfDayLate += 1;

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
                if (HalfDayFlag == 2)
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

                return Result<AttendanceDeleteResponseModel>.Success(null,"Attendance successfully deleted!");

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
}
