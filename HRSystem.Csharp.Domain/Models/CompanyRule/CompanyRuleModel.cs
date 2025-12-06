using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Domain.Models.CompanyRule;

public class CompanyRuleListResponseModel : PagedResult<CompanyRuleModel>
{
}

public class CompanyRuleListRequestModel : PaginationRequestModel
{
    public string? RuleDescription { get; set; }
}

public class CompanyRuleModel
{
    public string? CompanyRuleId { get; set; }
    public string? CompanyRuleCode { get; set; }
    public string? Description { get; set; }
    public string? Value { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool DeleteFlag { get; set; }
}

public class RuleUpdateRequestModel
{
    public string? CompanyRuleCode { get; set; }
    public string? Description { get; set; }
    public string? Value { get; set; }
}