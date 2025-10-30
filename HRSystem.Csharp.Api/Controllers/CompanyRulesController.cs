using HRSystem.Csharp.Domain.Features;
using HRSystem.Csharp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyRulesController : Controller
    {
        private readonly BL_CompanyRules_ _blCompanyRules;
        public CompanyRulesController(BL_CompanyRules_ blCompanyRules)
        {
            _blCompanyRules = blCompanyRules;
        }

        [HttpGet("GetAllCompanyRules")]
        public async Task<IActionResult> GetAllCompanyRules()
        {
            var result = await _blCompanyRules.GetAllCompanyRulesAsync();
            return Ok(result);

        }

        [HttpPost("UpdateCompanyRule")]
        public async Task<IActionResult> UpdateCompanyRule(CompanyRules companyRules)
        {
            var result = await _blCompanyRules.Update(companyRules);
            return Ok(result);
        }
    }
}