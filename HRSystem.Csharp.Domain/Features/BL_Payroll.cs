using HRSystem.Csharp.Domain.Models.PayRoll;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace HRSystem.Csharp.Domain.Features
{
    public class BL_Payroll
    {

        private readonly DA_Payroll _daPayroll;

        public BL_Payroll(DA_Payroll daPayroll)
        {
            _daPayroll = daPayroll;
        }

        public Result<List<PayrollResponseModel>> GetAllPayroll()
        {
            var payrolls = _daPayroll.GetAllPayroll();
            return Result<List<PayrollResponseModel>>.Success(payrolls.Data);
        }

        public Result<List<PayrollEditResponseModel>> EditPayroll(string payrollCode)
        {
            var payroll = _daPayroll.EditPayroll(payrollCode);
            return Result<List<PayrollEditResponseModel>>.Success(payroll.Data);
        }

        public Result<PayrollCreateResponseModel> CreatePayroll(PayrollCreateRequestModel req)
        {
            return _daPayroll.CreatePayroll(req);
        }

        public Result<PayrollUpdateResponseModel> UpdatePayroll(string payrollCode, PayrollUpdateRequestModel req)
        {
            return _daPayroll.UpdatePayroll(payrollCode, req);
        }

        public Result<PayrollDeleteResponseModel> DeletePayroll(string payrollCode)
        {
            return _daPayroll.DeletePayroll(payrollCode);
        }
    }
}
