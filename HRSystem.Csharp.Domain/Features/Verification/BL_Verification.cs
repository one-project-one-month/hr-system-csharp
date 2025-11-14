namespace HRSystem.Csharp.Domain.Features.Verification;

public class BL_Verification : AuthorizationService
{
    private readonly DA_Verification _da;
    private readonly EmailService _emailService;
    private readonly ILogger<BL_Verification> _logger;

    public BL_Verification(IHttpContextAccessor httpContextAccessor,
        DA_Verification da,
        EmailService emailService,
        ILogger<BL_Verification> logger) : base(httpContextAccessor)
    {
        _da = da;
        _emailService = emailService;
        _logger = logger;
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
                CreatedBy = UserCode,
                CreatedAt = DateTime.Now,
                DeleteFlag = false
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

    public async Task<Result<bool>> VerifyCode(VerifiyCodeRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.Email) || string.IsNullOrEmpty(requestModel.VerificationCode))
        {
            return Result<bool>.ValidationError("Email or Code is missing.");
        }

        try
        {
            var data = await _da.GetActiveByEmailAsync(requestModel.Email);
            if (data is null)
            {
                return Result<bool>.NotFoundError("No active verification found.");
            }

            if (DateTime.Now > data.ExpiredTime)
            {
                return Result<bool>.ValidationError("Verification code expired!");
            }

            if (data.VerificationCode == requestModel.VerificationCode)
            {
                data.IsUsed = true;
                await _da.UpdateAsync(data);
                return Result<bool>.Success(true, "Verification successful!");
            }

            return Result<bool>.Success(false, "Invalid verification code!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<bool>.SystemError(ex.Message);
        }
    }
}