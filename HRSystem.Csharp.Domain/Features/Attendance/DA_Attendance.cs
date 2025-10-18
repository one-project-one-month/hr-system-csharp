using HRSystem.Csharp.Domain.Models.Attendance;
using HRSystem.Csharp.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<Result<AttendanceListResponseModel>> List()
        {
            try
            {
                var attendanceList = await _db.TblAttendances.Where(x => x.DeleteFlag == false)
                .OrderByDescending(x => x.AttendanceDate)
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

                var newAttendance = new TblAttendance()
                {
                    AttendanceId = Guid.NewGuid(),
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
                    FullDayFlag = null,
                    Remark = requestModel.Remark,
                    IsSavedLocation = null,
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

        public static TimeSpan CalculateWorkingHours(DateTime checkIn, DateTime checkOut)
        {
            // Define office hours (same day)
            DateTime officeStart = checkIn.Date.AddHours(9);   // 9:00 AM
            DateTime officeEnd = checkIn.Date.AddHours(17);  // 5:00 PM

            // Clamp times to office hours
            if (checkIn < officeStart)
                checkIn = officeStart;
            if (checkOut > officeEnd)
                checkOut = officeEnd;

            // If invalid (checked out before check-in or outside office time)
            if (checkOut < checkIn)
                return TimeSpan.Zero;

            return checkOut - checkIn;
        }

        public static int CalculateHourlyLate(DateTime checkIn, DateTime checkOut)
        {
            DateTime officeStart = checkIn.Date.AddHours(9);  // 9:00 AM
            DateTime MorningFirstLate = checkIn.Date.AddHours(9.5); // 9:30 AM
            DateTime MorningSecondLate = checkIn.Date.AddHours(10);  // 10:00 AM

            int hourLate = 0;

            // If check-in between 9:30 and 10:00 -> 1 hour late
            if (checkIn > MorningFirstLate && checkIn <= MorningSecondLate)
                hourLate = 1;


            DateTime officeEnd = checkIn.Date.AddHours(17);  // 5:00 PM
            DateTime eveningFirstLate = checkIn.Date.AddHours(16.5); // 4:30 PM
            DateTime eveningSecondLate = checkIn.Date.AddHours(16);  // 4:00 PM

            if (checkOut < eveningFirstLate && checkOut >= eveningSecondLate)
                hourLate += 1;

            return hourLate;
        }

        public static int CalculateHalfDayLate(DateTime checkIn, DateTime checkOut)
        {
            int halfDayLate = 0;

            //For Morning Part
            DateTime MorningLate = checkIn.Date.AddHours(10); // 10:00 AM
            if (checkIn > MorningLate)
                halfDayLate = 1;

            //For Evening Part
            DateTime eveningLate = checkIn.Date.AddHours(16);  // 4:00 PM

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

                item.EmployeeCode = requestModel.EmployeeCode;
                item.AttendanceDate = requestModel.AttendanceDate;
                item.CheckInTime = requestModel.CheckInTime;
                item.CheckInLocation = requestModel.CheckInLocation;
                item.CheckOutTime = requestModel.CheckOutTime;
                item.CheckOutLocation = requestModel.CheckOutLocation;
                item.WorkingHour = requestModel.WorkingHour;
                item.HourLateFlag = requestModel.HourLateFlag;
                item.HalfDayFlag = requestModel.HalfDayFlag;
                item.FullDayFlag = requestModel.FullDayFlag;
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
