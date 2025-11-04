namespace HRSystem.Csharp.Domain.Models.Auth;

public class ChangePasswordRequestModel
{
    public string EmployeeCode { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}