using HRSystem.Csharp.Database.AppDbContextModels;
using HRSystem.Csharp.Domain.Models.PayRoll;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRSystem.Csharp.Domain.Features
{
    public class DA_Payroll
    {
        private readonly AppDbContext _appDbContext;
        private readonly string currentUser = "Admin"; // Testing Only

        public DA_Payroll(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Result<List<PayrollResponseModel>> GetAllPayroll()
        {
            try
            {
                var result = _appDbContext.TblPayrolls
                    .Where(p => !p.DeleteFlag.GetValueOrDefault())
                    .Select(p => new PayrollResponseModel
                    {
                        PayrollCode = p.PayrollCode,
                        EmployeeCode = p.EmployeeCode,
                        PayrollDate = p.PayrollDate,
                        Status = p.PayrollStatus,
                        BaseSalary = p.BaseSalary,
                        Allowance = p.Allowance,
                        GrossPay = p.BaseSalary + p.Allowance ?? 0, // Example, adjust as needed
                        Deduction = p.Deduction,
                        Tax = p.Tax,
                        Bonus = p.Bonus,
                        NetPay = p.GrandTotalPayroll
                    }).ToList();

                return Result<List<PayrollResponseModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<PayrollResponseModel>>.Error($"Error retrieving payrolls: {ex.Message}");
            }
        }

        public Result<List<PayrollEditResponseModel>> EditPayroll(string payrollCode)
        {
            try
            {
                var result = _appDbContext.TblPayrolls
                    .Where(p => p.PayrollCode == payrollCode && !p.DeleteFlag.GetValueOrDefault())
                    .Select(p => new PayrollEditResponseModel
                    {
                        PayrollCode = p.PayrollCode,
                        EmployeeCode = p.EmployeeCode,
                        PayrollDate = p.PayrollDate,
                        Status = p.PayrollStatus,
                        TotalWorkingHour = p.TotalWorkingHour,
                        LeaveHour = p.LeaveHour,
                        ActualWorkingHour = p.ActualWorkingHour,
                        BaseSalary = p.BaseSalary,
                        Allowance = p.Allowance,
                        GrossPay = p.BaseSalary + p.Allowance ?? 0,
                        Deduction = p.Deduction,
                        Tax = p.Tax,
                        Bonus = p.Bonus,
                        NetPay = p.GrandTotalPayroll
                    }).ToList();

                return Result<List<PayrollEditResponseModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<PayrollEditResponseModel>>.Error($"Error retrieving payroll: {ex.Message}");
            }
        }

        public Result<PayrollCreateResponseModel> CreatePayroll(PayrollCreateRequestModel req)
        {
            try
            {
                var newPayroll = new TblPayroll
                {
                    PayrollId = Guid.NewGuid().ToString(),
                    PayrollCode = req.PayrollCode,
                    EmployeeCode = req.EmployeeCode,
                    PayrollDate = req.PayrollDate ?? DateTime.Now,
                    PayrollStatus = req.Status,
                    TotalWorkingHour = req.TotalWorkingHour ?? 0,
                    LeaveHour = req.LeaveHour ?? 0,
                    ActualWorkingHour = req.ActualWorkingHour ?? 0,
                    BaseSalary = req.BaseSalary ?? 0,
                    Allowance = req.Allowance ?? 0,
                    Deduction = req.Deduction ?? 0,
                    Tax = req.Tax ?? 0,
                    Bonus = req.Bonus ?? 0,
                    GrandTotalPayroll = req.NetPay ?? 0,
                    CreatedBy = currentUser,
                    CreatedAt = DateTime.UtcNow,
                    DeleteFlag = false
                };

                _appDbContext.TblPayrolls.Add(newPayroll);
                _appDbContext.SaveChanges();

                return Result<PayrollCreateResponseModel>.Success(new PayrollCreateResponseModel(), "Payroll Created Successfully");
            }
            catch (Exception ex)
            {
                return Result<PayrollCreateResponseModel>.Error($"Error creating payroll: {ex.Message}");
            }
        }

        public Result<PayrollUpdateResponseModel> UpdatePayroll(string payrollCode, PayrollUpdateRequestModel req)
        {
            var existing = _appDbContext.TblPayrolls.FirstOrDefault(p => p.PayrollCode == payrollCode && !p.DeleteFlag.GetValueOrDefault());
            if (existing == null)
                return Result<PayrollUpdateResponseModel>.NotFoundError("Payroll not found");

            existing.EmployeeCode = req.EmployeeCode ?? existing.EmployeeCode;
            existing.PayrollDate = req.PayrollDate ?? existing.PayrollDate;
            existing.PayrollStatus = req.Status ?? existing.PayrollStatus;
            existing.TotalWorkingHour = req.TotalWorkingHour ?? existing.TotalWorkingHour;
            existing.LeaveHour = req.LeaveHour ?? existing.LeaveHour;
            existing.ActualWorkingHour = req.ActualWorkingHour ?? existing.ActualWorkingHour;
            existing.BaseSalary = req.BaseSalary ?? existing.BaseSalary;
            existing.Allowance = req.Allowance ?? existing.Allowance;
            existing.Deduction = req.Deduction ?? existing.Deduction;
            existing.Tax = req.Tax ?? existing.Tax;
            existing.Bonus = req.Bonus ?? existing.Bonus;
            existing.GrandTotalPayroll = req.NetPay ?? existing.GrandTotalPayroll;
            existing.ModifiedBy = currentUser;
            existing.ModifiedAt = DateTime.UtcNow;

            _appDbContext.SaveChanges();

            return Result<PayrollUpdateResponseModel>.Success(new PayrollUpdateResponseModel(), "Payroll Updated Successfully");
        }

        public Result<PayrollDeleteResponseModel> DeletePayroll(string payrollCode)
        {
            var model = _appDbContext.TblPayrolls.FirstOrDefault(p => p.PayrollCode == payrollCode && !p.DeleteFlag.GetValueOrDefault());
            if (model == null)
                return Result<PayrollDeleteResponseModel>.NotFoundError("Payroll not found");

            model.DeleteFlag = true;
            model.ModifiedBy = currentUser;
            model.ModifiedAt = DateTime.UtcNow;

            _appDbContext.SaveChanges();

            return Result<PayrollDeleteResponseModel>.Success(new PayrollDeleteResponseModel(), "Payroll Deleted Successfully");
        }
    }
}
