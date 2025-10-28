using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features
{
    public class BL_CompanyRules_
    {
        private readonly DA_CompanyRules _daCompanyRules;
        public BL_CompanyRules_(DA_CompanyRules daCompanyRules)
        {
            _daCompanyRules = daCompanyRules;
        }

        public async Task<Result<List<CompanyRules>>> GetAllCompanyRulesAsync()
        {
            var res = await _daCompanyRules.GetAllCompanyRulesAsync();

            return res;
        }

        public async Task<Result<CompanyRules>> Update(CompanyRules companyRule)
        {
            var res = await _daCompanyRules.UpdateCompanyRuleAsync(companyRule);
            return res;
        }
    }
}
