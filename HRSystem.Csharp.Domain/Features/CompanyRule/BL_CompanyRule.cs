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

    public async Task<Result<CompanyRuleResponseModel>> GetCompanyRuleByIdAysnc (string code)
    {
        if (code is null)
            return Result<CompanyRuleResponseModel>.BadRequestError("Code is required!");

        var result = await _daCompanyRules.GetCompanyRuleByIdAsync(code);
        if (result is null)
            return Result<CompanyRuleResponseModel>.NotFoundError("Company Rule is not found");
        var response = new CompanyRuleResponseModel
        {
            CompanyRuleCode = result.CompanyRuleCode,
            Description = result.Description,
            Value = result.Value,
            IsActive = result.IsActive,
        };

        return Result<CompanyRuleResponseModel>.Success(response);
    }
    public async Task<Result<bool>> Update(RuleUpdateRequestModel reqModel)
    {
        var res = await _daCompanyRules.UpdateCompanyRuleAsync(reqModel);
        return res;
    }
}