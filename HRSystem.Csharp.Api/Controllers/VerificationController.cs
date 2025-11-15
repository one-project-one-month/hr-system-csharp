using HRSystem.Csharp.Domain.Features.Verification;
using HRSystem.Csharp.Domain.Models.Verification;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerificationController : ControllerBase
{
    private readonly BL_Verification _blVerification;

    public VerificationController(BL_Verification blVerification)
    {
        _blVerification = blVerification;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetVerificationList()
    {
        var result = await _blVerification.List();
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetVerificationById(string id)
    {
        var result = await _blVerification.GetById(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("get-by-email/{email}")]
    public async Task<IActionResult> GetVerificationByEmail(string email)
    {
        var result = await _blVerification.GetByEmail(email);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("send-verification-mail")]
    public async Task<IActionResult> Create([FromBody] VerificationRequestModel reqModel)
    {
        var result = await _blVerification.Create(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("verify-code")]
    public async Task<IActionResult> VerifyCode([FromBody] VerifiyCodeRequestModel reqModel)
    {
        var result = await _blVerification.VerifyCode(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}