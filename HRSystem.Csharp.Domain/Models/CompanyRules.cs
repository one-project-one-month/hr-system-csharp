using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class CompanyRules
    {
        public string? CompanyRuleId { get; set; }
        public string? CompanyRuleCode { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? DeleteFlag { get; set; }

        public CompanyRules()
        {
            CompanyRuleId = null;
            CompanyRuleCode = "";
            Description = "";
            Value = "";
            IsActive = true;
            CreatedAt = null;
            CreatedBy = "";
            ModifiedAt = null;
            ModifiedBy = "";
            DeleteFlag = false;
        }
    }
}
