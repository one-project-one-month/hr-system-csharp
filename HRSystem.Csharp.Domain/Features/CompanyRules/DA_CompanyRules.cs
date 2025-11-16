using HRSystem.Csharp.Domain.Models.CompanyRules;

namespace HRSystem.Csharp.Domain.Features.Rule;

public class DA_CompanyRules
{
    private readonly AppDbContext _context;

    public DA_CompanyRules(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CompanyRuleListResponseModel>> GetAllCompanyRulesAsync(
        CompanyRuleListRequestModel reqModel)
    {
        try
        {
            var query = _context.TblCompanyRules
                .AsNoTracking()
                .Where(r => !r.DeleteFlag);

            if (!string.IsNullOrWhiteSpace(reqModel.RuleDescription))
            {
                query = query.Where(r => r.Description != null
                                         && r.Description.ToLower() == reqModel.RuleDescription.ToLower());
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            var rules = query.Select(cr => new CompanyRules
            {
                CompanyRuleId = cr.CompanyRuleId,
                CompanyRuleCode = cr.CompanyRuleCode,
                Description = cr.Description,
                Value = cr.Value,
                IsActive = cr.IsActive == null ? false : cr.IsActive,
                CreatedAt = cr.CreatedAt,
                CreatedBy = cr.CreatedBy,
                ModifiedAt = cr.ModifiedAt,
                ModifiedBy = cr.ModifiedBy,
                DeleteFlag = cr.DeleteFlag == null ? false : cr.DeleteFlag
            });

            var pagedResult = await rules.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new CompanyRuleListResponseModel()
            {
                Items = pagedResult.Items ?? new List<CompanyRules>(),
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<CompanyRuleListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<CompanyRuleListResponseModel>.Error(
                $"An error occurred while retrieving company rules: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateCompanyRuleAsync(RuleUpdateRequestModel reqModel)
    {
        try
        {
            var existingRule = await _context.TblCompanyRules
                .FirstOrDefaultAsync(cr => cr.CompanyRuleCode == reqModel.CompanyRuleCode);

            if (existingRule == null)
            {
                return Result<bool>.NotFoundError("Company rule not found.");
            }

            existingRule.Description = reqModel.Description;
            existingRule.Value = reqModel.Value;
            existingRule.ModifiedAt = DateTime.UtcNow;
            existingRule.ModifiedBy = "admin";

            _context.TblCompanyRules.Update(existingRule);
            await _context.SaveChangesAsync();
            return Result<bool>.Success("Rule updated successfully!");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"An error occurred while updating the company rule: {ex.Message}");
        }
    }
}