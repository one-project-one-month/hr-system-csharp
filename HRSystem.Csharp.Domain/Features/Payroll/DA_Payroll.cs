using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Payroll
{
    public class DA_Payroll
    {
        private readonly AppDbContext _appDbContext;
        public DA_Payroll(AppDbContext appDbContext) { 
            _appDbContext = appDbContext;
        } 

    }
}
