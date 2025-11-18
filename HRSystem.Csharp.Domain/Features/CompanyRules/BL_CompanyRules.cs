using HRSystem.Csharp.Domain.Models.CompanyRules;

namespace HRSystem.Csharp.Domain.Features.Rule;

public class BL_CompanyRules
{
    private readonly DA_CompanyRules _daCompanyRules;

    public BL_CompanyRules(DA_CompanyRules daCompanyRules)
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