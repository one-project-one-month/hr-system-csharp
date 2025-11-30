using HRSystem.Csharp.Domain.Models.Attendance;

namespace HRSystem.Csharp.Domain.Features.Attendance;

public class BL_Attendance
{
    private readonly DA_Attendance _attendance;

    public BL_Attendance(DA_Attendance attendance)
    {
        _attendance = attendance;
    }

    public async Task<Result<AttendanceListResponseModel>> List(String? EmpName, DateTime startDate, DateTime endDate, int pageNo, int PageSize)
    {
        var data = await _attendance.List(EmpName, startDate, endDate, pageNo, PageSize);
        return data;
    }

    public async Task<Result<AttendanceCreateResponseModel>> Create(string userId,AttendanceCreateRequestModel requestModel)
    {
        var data = await _attendance.Create(userId,requestModel);
        return data;
    }

    public async Task<Result<AttendanceUpdateResponseModel>> Update(string userId, AttendanceUpdateRequestModel requestModel)
    {
        var data = await _attendance.Update(userId,requestModel);
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