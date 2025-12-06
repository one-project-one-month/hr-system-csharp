using HRSystem.Csharp.Domain.Models.CompanyRule;

namespace HRSystem.Csharp.Domain.Features.CompanyRule;

public class BL_CompanyRule
{
    private readonly DA_CompanyRule _daCompanyRules;

    public BL_CompanyRule(DA_CompanyRule daCompanyRules)
    {
        _daCompanyRules = daCompanyRules;
    }

    public async Task<Result<CompanyRuleListResponseModel>> GetAllCompanyRulesAsync(
        CompanyRuleListRequestModel reqModel)
    {
        var result = await _daCompanyRules.GetAllCompanyRulesAsync(reqModel);
        return result;
    }

    public async Task<Result<bool>> Update(RuleUpdateRequestModel reqModel)
    {
        var res = await _daCompanyRules.UpdateCompanyRuleAsync(reqModel);
        return res;
    }
}