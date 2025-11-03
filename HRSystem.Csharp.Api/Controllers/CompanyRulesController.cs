using HRSystem.Csharp.Domain.Features.Rule;
using HRSystem.Csharp.Domain.Models;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyRulesController : Controller
    {
        private readonly BL_CompanyRules _blCompanyRules;

        public CompanyRulesController(BL_CompanyRules blCompanyRules)
        {
            _blCompanyRules = blCompanyRules;
        }

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