using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace HRSystem.Csharp.Domain.Helpers;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IFluentEmail _emailSender;

    public EmailService(ILogger<EmailService> logger, IFluentEmail emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task<bool> SendEmailVerification(EmailModel requestModel)
    {
        try
        {
            var emailResult = await _emailSender
                                .To(requestModel.Email)
                               .Subject(requestModel.Subject)
                               .Body(requestModel.Body, isHtml: true)
                               .SendAsync();
            if (emailResult.Successful is false)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return false;
        }

        return true;
    }
}

public class EmailModel
{
    public string Email { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
}