using HRSystem.Csharp.Domain.Models;

namespace HRSystem.Csharp.Domain.Features.Rule;

public class BL_CompanyRules
{
    private readonly DA_CompanyRules _daCompanyRules;
    public BL_CompanyRules(DA_CompanyRules daCompanyRules)
    {
        _daCompanyRules = daCompanyRules;
    }

    public async Task<Result<CompanyRulesInfoModel>> GetAllCompanyRulesAsync()
    {
        var res = await _daCompanyRules.GetAllCompanyRulesAsync();
            
        if (res.Data.All(cr => cr.IsActive == false))
        {
            return Result<CompanyRulesInfoModel>.ValidationError("All company rules are inactive.");
        }

        if (res.Data.All(cr => cr.DeleteFlag == true))
        {
            return Result<CompanyRulesInfoModel>.ValidationError("All company ruls are deleted.");
        }

        CompanyRulesInfoModel companyRulesInfo = new CompanyRulesInfoModel();
        companyRulesInfo.OfficeStartTime = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "OfficeStartTime")?.Value;
        companyRulesInfo.OfficeEndTime = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "OfficeEndTime")?.Value;
        companyRulesInfo.OfficeBreakHour = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "OfficeBreakHour")?.Value;
        companyRulesInfo.CheckinAcceptable = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "CheckinAcceptable")?.Value;
        companyRulesInfo.CheckinOneHourLate = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "CheckinOneHourLate")?.Value;
        companyRulesInfo.CheckoutAcceptable = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "CheckoutAcceptable")?.Value;
        companyRulesInfo.CheckoutHourLate = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "CheckoutHourLate")?.Value;
        companyRulesInfo.MorningHalfCheckinAcceptable = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "MorningHalfCheckinAcceptable")?.Value;
        companyRulesInfo.MorningHalfCheckinHourLate = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "MorningHalfCheckinHourLate")?.Value;
        companyRulesInfo.EveningHalfCheckoutAcceptable = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "EveningHalfCheckoutAcceptable")?.Value;
        companyRulesInfo.EveningHalfCheckoutHourLate = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "EveningHalfCheckoutHourLate")?.Value;
        companyRulesInfo.HourLateFlagDeduction = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "HourLateFlagDeduction")?.Value;
        companyRulesInfo.HalfDayFlagDeduction = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "HalfDayFlagDeduction")?.Value;
        companyRulesInfo.FullDayFlagDeduction = res.Data.FirstOrDefault(cr => cr.CompanyRuleCode == "FullDayFlagDeduction")?.Value;


        return Result<CompanyRulesInfoModel>.Success(companyRulesInfo);
    }

    public async Task<Result<CompanyRules>> Update(CompanyRules companyRule)
    {
        var res = await _daCompanyRules.UpdateCompanyRuleAsync(companyRule);
        return res;
    }
}