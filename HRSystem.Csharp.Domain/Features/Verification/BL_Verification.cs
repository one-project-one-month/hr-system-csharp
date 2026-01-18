using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace HRSystem.Csharp.Domain.Features.Verification;

public class BL_Verification : AuthorizationService
{
    private readonly DA_Verification _da;
    private readonly EmailService _emailService;
    private readonly ILogger<BL_Verification> _logger;
    private readonly DA_Employee _daEmployee;
    private readonly JwtService _jwtService;

    public BL_Verification(IHttpContextAccessor httpContextAccessor,
        DA_Verification da,
        EmailService emailService,
        ILogger<BL_Verification> logger,
        DA_Employee daEmployee,
        JwtService jwtService
        ) : base(httpContextAccessor)
    {
        _da = da;
        _emailService = emailService;
        _logger = logger;
        _daEmployee = daEmployee;
        _jwtService = jwtService;
    }

    public async Task<Result<VerificationResponseModel>> List()
    {
        try
        {
            var data = await _da.GetAllAsync();
            if (!data.Any())
            {
                return Result<VerificationResponseModel>.NotFoundError("No Verification Found.");
            }

            var model = new VerificationResponseModel
            {
                VerificationCodes = data.Select(VerificationModel.FromTblVerification).ToList()
            };
            return Result<VerificationResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VerificationResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<VerificationResponseModel>> GetById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Result<VerificationResponseModel>.ValidationError("Id cannot be null.");
        }

        try
        {
            var data = await _da.GetByIdAsync(id);
            if (data is null)
            {
                return Result<VerificationResponseModel>.NotFoundError($"No record found for {id}");
            }

            var model = new VerificationResponseModel
            {
                VerificationCode = VerificationModel.FromTblVerification(data)
            };
            return Result<VerificationResponseModel>.Success(model, "Verification found!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VerificationResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<VerificationResponseModel>> GetByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return Result<VerificationResponseModel>.ValidationError("Email cannot be null.");

        try
        {
            var data = await _da.GetByEmailAsync(email);
            if (!data.Any())
            {
                return Result<VerificationResponseModel>.NotFoundError($"No verification for {email}");
            }

            var model = new VerificationResponseModel
            {
                VerificationCodes = data.Select(VerificationModel.FromTblVerification).ToList()
            };
            return Result<VerificationResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VerificationResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<VerificationResponseModel>> Create(VerificationRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.Email) || !requestModel.Email.IsValidEmail())
        {
            return Result<VerificationResponseModel>.ValidationError("Invalid email.");
        }

        try
        {
            string otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.Now.AddMinutes(3);

            var entity = new TblVerification
            {
                VerificationId = Ulid.NewUlid().ToString(),
                VerificationCode = otp,
                Email = requestModel.Email,
                ExpiredTime = expiry,
                CreatedBy = UserCode!,
                CreatedAt = DateTime.Now,
                DeleteFlag = false,
                IsUsed = false
            };

            var added = await _da.AddAsync(entity);
            if (!added)
            {
                return Result<VerificationResponseModel>.SystemError("Failed to save verification code to database.");
            }

            var emailTemplate = new EmailModel
            {
                Email = requestModel.Email,
                Subject = EmailSubjectTemplates.Verification,
                Body = EmailBodyTemplates.Otp.Replace("(@otp)", otp)
            };

            var sent = await _emailService.SendEmailVerification(emailTemplate);
            if (!sent)
            {
                return Result<VerificationResponseModel>.SystemError("Failed to send email.");
            }

            return Result<VerificationResponseModel>.Success("Verification code sent successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VerificationResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<VerificationResponseModel>> SendEmail(VerificationRequestModel requestModel, string employeeCode)
    {
        var userCode = employeeCode;

        if (string.IsNullOrEmpty(requestModel.Email) || !requestModel.Email.IsValidEmail())
        {
            return Result<VerificationResponseModel>.ValidationError("Invalid email.");
        }

        try
        {
            string otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.Now.AddMinutes(3);

            var entity = new TblVerification
            {
                VerificationId = Ulid.NewUlid().ToString(),
                VerificationCode = otp,
                Email = requestModel.Email,
                ExpiredTime = expiry,
                CreatedBy = userCode!,
                CreatedAt = DateTime.Now,
                DeleteFlag = false,
                IsUsed = false
            };

            var added = await _da.AddAsync(entity);
            if (!added)
            {
                return Result<VerificationResponseModel>.SystemError("Failed to save new passcode to database.");
            }

            var emailTemplate = new EmailModel
            {
                Email = requestModel.Email,
                Subject = EmailSubjectTemplates.Verification,
                Body = EmailBodyTemplates.ForgetPassword.Replace("(@otp)", otp)
            };

            var sent = await _emailService.SendEmailVerification(emailTemplate);
            if (!sent)
            {
                return Result<VerificationResponseModel>.SystemError("Failed to send email.");
            }

            return Result<VerificationResponseModel>.Success("New Passcode sent to email successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VerificationResponseModel>.SystemError(ex.Message);
        }
    }


    public static string GenerateSecureResetToken(int length = 32)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(bytes)
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "");
    }


    public async Task<Result<ResetTokenResponse>> VerifyCode(VerifiyCodeRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.Email) || string.IsNullOrEmpty(requestModel.VerificationCode))
        {
            return Result<ResetTokenResponse>.ValidationError("Email or Code is missing.");
        }

        try
        {
            var data = await _da.GetActiveByEmailAsync(requestModel.Email);
            if (data is null)
            {
                return Result<ResetTokenResponse>.NotFoundError("No active verification found.");
            }

            if (DateTime.Now > data.ExpiredTime)
            {
                return Result<ResetTokenResponse>.ValidationError("Verification code expired!");
            }

            if (data.VerificationCode == requestModel.VerificationCode)
            {
                data.IsUsed = true;
                await _da.UpdateAsync(data);
                string resetToken = GenerateSecureResetToken();

                // Save token to database, linked to the employee
                await _da.AddResetTokenAsync(new TblResetPasswordToken
                {
                    Email = requestModel.Email,
                    Token = resetToken,
                    ExpiresAt = DateTime.Now.AddMinutes(10),
                    IsUsed = false,
                    DeleteFlag = false,
                    CreatedAt = DateTime.Now
                });

                return Result<ResetTokenResponse>.Success(
                    new ResetTokenResponse { ResetToken = resetToken },
                    "Verification successful!"
                );
            }

            return Result<ResetTokenResponse>.Error("Invalid verification code!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<ResetTokenResponse>.SystemError(ex.Message);
        }
    }
    

    public async Task<Result<bool>> ResetPassword(ResetPasswordRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.ResetToken) || string.IsNullOrEmpty(requestModel.NewPassword) || string.IsNullOrEmpty(requestModel.Email))
        {
            return Result<bool>.ValidationError("Reset Token or new Password or Email is missing.");
        }

        var Employee = await _daEmployee.GetEmployeeByEmail(requestModel.Email);

        if (Employee == null)
            return Result<bool>.ValidationError("Employee not found with the email!");

        try
        {
            var data = await _da.GetActiveRestTokenByEmailAsync(requestModel.Email);
            if (data is null)
            {
                return Result<bool>.NotFoundError("No active reset token found.");
            }

            if (DateTime.Now > data.ExpiresAt)
            {
                return Result<bool>.ValidationError("Reset Token expired!");
            }

            if (data.Token == requestModel.ResetToken)
            {
                data.IsUsed = true;
                await _da.UpdateResetTokenAsync(data);

                _jwtService.HashPassword(requestModel.NewPassword);
                Employee.Password  = _jwtService.HashPassword(requestModel.NewPassword);
                await _daEmployee.UpdateEmployee(Employee);
                return Result<bool>.Success("Reset Password Successful!");
            }

            return Result<bool>.Error("Invalid reset token!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<bool>.SystemError(ex.Message);
        }
    }


}