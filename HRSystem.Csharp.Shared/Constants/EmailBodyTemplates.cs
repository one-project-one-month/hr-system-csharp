namespace HRSystem.Csharp.Shared.Constants;

public class EmailBodyTemplates
{
    public static string Otp { get; } = "Your Verification Code is: <b>(@otp)</b>.";
    public static string ForgetPassword { get; } = "Your New Passcode is: <b>(@otp)</b>.";
}