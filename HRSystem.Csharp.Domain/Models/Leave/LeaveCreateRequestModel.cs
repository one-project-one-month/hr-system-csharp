namespace HRSystem.Csharp.Domain.Models.Leave;

public class LeaveCreateRequestModel
{
    public string EmployeeCode { get; set; } = null!;
    public EnumLeaveType LeaveType { get; set; }
    public string Reason { get; set; } = null!;
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public decimal? TotalHours { get; set; }
    public bool? IsPaid { get; set; }
    public string Status { get; set; } = null!;
    public EnumFullOrHalfLeave FullOrHalf { get; set; }
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
    Reject
}

public class AvailableLeaveRequestModel
{
    public EnumLeaveType LeaveType { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}

public class AvailableLeaveResponseModel
{
    public int TotalLeavenTaken { get; set; }
    public int RemainingLeave { get; set; }
    public int LeaveAllowedPerYear { get; set; }
    public EnumLeaveType LeaveType { get; set; }
}

public class LeaveCreateResponseModel
{
}