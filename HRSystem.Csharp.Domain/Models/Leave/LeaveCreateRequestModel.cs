using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Domain.Models.Leave;

public class LeaveCreateRequestModel
{
    public EnumLeaveType LeaveType { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public EnumFullOrHalfLeave FullOrHalf { get; set; }
    public string Reason { get; set; } = null!;
}

public enum EnumLeaveType
{
    MedicalLeave,
    CasualLeave,
    LeaveWithoutPay,
    EarnLeave,
    MaternityLeave
}

public enum EnumFullOrHalfLeave
{
    HalfLeave,
    FullLeave
}

public enum EnumLeaveStatus
{
    Pending,
    Approved,
    Rejected
}

public class AvailableLeaveRequestModel
{
    public EnumLeaveType LeaveType { get; set; }
    /*public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }*/
}

public class AvailableLeaveResponseModel
{
    public int TotalLeavenTaken { get; set; }
    public int RemainingLeave { get; set; }
    public int LeaveAllowedPerYear { get; set; }
    public EnumLeaveType LeaveType { get; set; }
}

public class LeaveApproveRequestModel
{
    public string LeaveCode { get; set; }
}

public class LeaveRejectRequestModel
{
    public string LeaveCode { get; set; }
}

public class LeaveListRequestModel : PaginationRequestModel
{
    public string? LeaveType { get; set; }
    public string? Query { get; set; }
}

public class EmployeeLeaveListRequestModel : PaginationRequestModel
{
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public string? LeaveType { get; set; }
}