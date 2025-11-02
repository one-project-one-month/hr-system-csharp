using HRSystem.Csharp.Domain.Models;

namespace HRSystem.Csharp.Domain.Features.Rule;

public class DA_CompanyRules
{
    private readonly AppDbContext _context;
    public DA_CompanyRules(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CompanyRules>>> GetAllCompanyRulesAsync()
    {
        try
        {
            var companyRules = _context.TblCompanyRules
                .Select(cr => new CompanyRules
                {
                    CompanyRuleId = cr.CompanyRuleId,
                    CompanyRuleCode = cr.CompanyRuleCode,
                    Description = cr.Description,
                    Value = cr.Value,
                    IsActive = cr.IsActive == null? false: cr.IsActive,
                    CreatedAt = cr.CreatedAt,
                    CreatedBy = cr.CreatedBy,
                    ModifiedAt = cr.ModifiedAt,
                    ModifiedBy = cr.ModifiedBy,
                    DeleteFlag = cr.DeleteFlag == null ? false : cr.DeleteFlag
                })
                .AsNoTracking().ToList();

            if (companyRules == null || companyRules.Count == 0)
            {
                return Result<List<CompanyRules>>.NotFoundError("No company rules found.");
            }
                
            return Result<List<CompanyRules>>.Success(companyRules);
        }
        catch (Exception ex)
        {
            return Result<List<CompanyRules>>.Error($"An error occurred while retrieving company rules: {ex.Message}");
        }
    }

    public async Task<Result<CompanyRules>> UpdateCompanyRuleAsync(CompanyRules companyRule)
    {
        try
        {
            var existingRule = _context.TblCompanyRules.FirstOrDefault(cr => cr.CompanyRuleId == companyRule.CompanyRuleId);
            if (existingRule == null)
            {
                return Result<CompanyRules>.NotFoundError("Company rule not found.");
            }
            existingRule.CompanyRuleCode = companyRule.CompanyRuleCode ?? string.Empty;
            existingRule.Description = companyRule.Description;
            existingRule.Value = companyRule.Value;
            existingRule.IsActive = companyRule.IsActive;
            existingRule.ModifiedAt = DateTime.UtcNow;
            existingRule.ModifiedBy = companyRule.ModifiedBy;
            existingRule.DeleteFlag = companyRule.DeleteFlag;

            _context.TblCompanyRules.Update(existingRule);
            await _context.SaveChangesAsync();
            return Result<CompanyRules>.Success(companyRule);
        }
        catch (Exception ex)
        {
            return Result<CompanyRules>.Error($"An error occurred while updating the company rule: {ex.Message}");
        }
    }
}