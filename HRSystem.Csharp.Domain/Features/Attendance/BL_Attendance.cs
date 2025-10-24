using HRSystem.Csharp.Domain.Models.Attendance;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Attendance
{
    public class BL_Attendance
    {
        private readonly DA_Attendance _attendance;

        public BL_Attendance(DA_Attendance attendance)
        {
            _attendance = attendance;
        }

        public async Task<Result<AttendanceListResponseModel>> List(int pageNo, int PageSize)
        {
            var data = await _attendance.List(pageNo, PageSize);
            return data;
        }

        public async Task<Result<AttendanceCreateResponseModel>> Create(AttendanceCreateRequestModel requestModel)
        {
            var data = await _attendance.Create(requestModel);
            return data;
        }

        public async Task<Result<AttendanceUpdateResponseModel>> Update(AttendanceUpdateRequestModel requestModel)
        {
            var data = await _attendance.Update(requestModel);
            return data;
        }

        public async Task<Result<AttendanceEditResponseModel>> Edit(string attendanceCode)
        {
            var data = await _attendance.Edit(attendanceCode);
            return data;
        }

        public async Task<Result<AttendanceDeleteResponseModel>> Delete(string attendanceCode)
        {
            var data = await _attendance.Delete(attendanceCode);
            return data;
        }

    }
}
