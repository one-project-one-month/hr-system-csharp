namespace HRSystem.Csharp.Domain.Features.Rule;

public class DA_Rule
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DA_Rule> _logger;

    public DA_Rule(AppDbContext appDbContext, ILogger<DA_Rule> logger)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }

    public async Task<Result<RuleResponseModel>> GetRuleByCode(string ruleCode)
    {
        try
        {
            var rule = await _appDbContext.TblCompanyRules
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.CompanyRuleCode == ruleCode);

            if (rule == null)
            {
                return Result<RuleResponseModel>.NotFoundError($"Rule with code {ruleCode} not found.");
            }

            var response = new RuleResponseModel
            {
                CompanyRuleCode = rule.CompanyRuleCode,
                Description = rule.Description,
                Value = rule.Value
            };

            return Result<RuleResponseModel>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Result<RuleResponseModel>.SystemError("Error getting rule!");
        }
    }
}

public class RuleResponseModel
{
    public string CompanyRuleCode { get; set; } = null!;
    public string? Description { get; set; }
    public string? Value { get; set; }
}

public class RuleCode
{
    public static string TotalCasualLeave { get; } = "RL001";
    public static string TotalMedicalLeave { get; } = "RL002";
    public static string TotalEarnLeave { get; } = "RL003";
    public static string TotalMaternityLeave { get; } = "RL004";
}