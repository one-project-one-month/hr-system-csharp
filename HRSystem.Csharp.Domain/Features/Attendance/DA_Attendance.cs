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

                var newAttendance = new TblAttendance()
                {
                    AttendanceId = Guid.NewGuid(),
                    AttendanceCode = Guid.NewGuid().ToString(),
                    EmployeeCode = requestModel.EmployeeCode,
                    AttendanceDate = requestModel.AttendanceDate,
                    CheckInTime = requestModel.CheckInTime,
                    CheckInLocation = requestModel.CheckInLocation,
                    CheckOutTime = requestModel.CheckOutTime,
                    CheckOutLocation = requestModel.CheckOutLocation,
                    WorkingHour = null,
                    HourLateFlag = null,
                    HalfDayFlag = null,
                    FullDayFlag = null,
                    Remark = requestModel.Remark,
                    IsSavedLocation = null,
                    CreatedBy = null,
                    CreatedAt = DateTime.Now,
                    DeleteFlag = false
                };
                await _db.AddAsync(newAttendance);
                await _db.SaveChangesAsync();

                return Result<AttendanceCreateResponseModel>.Success(null,"Attendance is successfully created");
            }
            catch (Exception ex)
            {
                return Result<AttendanceCreateResponseModel>.SystemError(ex.Message);
            }
        }




    }
}
