namespace HRSystem.Csharp.Domain.Models.Auth;

public class ChangePasswordRequestModel
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}