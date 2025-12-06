using HRSystem.Csharp.Domain.Features.CompanyRule;
using HRSystem.Csharp.Domain.Models.CompanyRule;
using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyRulesController(BL_CompanyRule blCompanyRules) : Controller
    {
        private readonly BL_CompanyRule _blCompanyRules = blCompanyRules;

        [HttpGet("list")]
        public async Task<IActionResult> GetAllCompanyRules([FromQuery] CompanyRuleListRequestModel reqModel)
        {
            var result = await _blCompanyRules.GetAllCompanyRulesAsync(reqModel);
            return Ok(result);
        }

        [HttpPost("update/{ruleCode}")]
        public async Task<IActionResult> UpdateCompanyRule(string ruleCode, RuleUpdateRequestModel reqModel)
        {
            reqModel.CompanyRuleCode = ruleCode;
            var result = await _blCompanyRules.Update(reqModel);
            return Ok(result);
        }
    }
}