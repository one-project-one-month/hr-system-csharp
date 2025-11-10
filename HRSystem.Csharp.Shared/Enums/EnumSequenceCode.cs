using System.ComponentModel;

namespace HRSystem.Csharp.Shared.Enums;

public enum EnumSequenceCode
{
    [Description("Role")] RL,
    [Description("User")] USR,
    [Description("Role Menu Permission")] RL_MENU_PM,
    [Description("Employee Project")] EMP_PRJ,
    [Description("Email Verification")] VER,
    [Description("Project")] PJ,
    [Description("Menu Item")] MI,
    [Description("Menu Group")] MG,
    [Description("Rule")] RULE,
    [Description("Task")] TASK,
    [Description("Employee")] EMP,
    [Description("Location")] LOC,
    [Description("Payroll")] PAY,
    [Description("Attendance")] ATT
}