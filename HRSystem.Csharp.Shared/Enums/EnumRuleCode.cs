using System.ComponentModel;

namespace HRSystem.Csharp.Shared.Enums;

public enum EnumRuleCode
{
    [Description("RL001")] TotalCasualLeave,
    [Description("RL002")] TotalMedicalLeave,
    [Description("RL003")] TotalEarnLeave,
    [Description("RL004")] TotalMaternityLeave,
    [Description("RL005")] FullWorkingHour,
    [Description("RL006")] HalfWorkingHour,
    [Description("RL007")] OfficeHourStartTime,
    [Description("RL008")] OfficeHourEndTime,
    [Description("RL009")] OfficeHourHalfTime,
    [Description("RL010")] CheckInAcceptable,
    [Description("RL011")] CheckOutAcceptable,
    [Description("RL012")] CheckInOneHourLate,
    [Description("RL013")] CheckOutOneHourLate
}