using HRSystem.Csharp.Domain.Features.Rule;
using HRSystem.Csharp.Domain.Models.Leave;
using Sprache;

namespace HRSystem.Csharp.Domain.Features.Leave;

public class BL_Leave : AuthorizationService
{
    private readonly DA_Leave _daLeave;
    private readonly ILogger<BL_Leave> _logger;
    private readonly DA_Employee _daEmployee;
    private readonly DA_Rule _daRule;
    private readonly DA_Sequence _daSequence;

    public BL_Leave(IHttpContextAccessor httpContextAccessor,
        DA_Leave daLeave,
        ILogger<BL_Leave> logger, DA_Employee daEmployee, DA_Rule daRule,
        DA_Sequence daSequence) : base(httpContextAccessor)
    {
        _daLeave = daLeave;
        _logger = logger;
        _daEmployee = daEmployee;
        _daRule = daRule;
        _daSequence = daSequence;
    }

    public async Task<Result<bool>> CreateLeave(LeaveCreateRequestModel reqModel)
    {
        try
        {
            #region Validation

            if (!Enum.IsDefined(typeof(EnumLeaveType), reqModel.LeaveType))
            {
                return Result<bool>.ValidationError("Invalid Leave Type");
            }

            if (string.IsNullOrWhiteSpace(reqModel.Reason))
            {
                return Result<bool>.ValidationError("Reason is required");
            }

            if (reqModel.FromDate == default)
            {
                return Result<bool>.ValidationError("FromDate is required.");
            }

            if (reqModel.ToDate == default)
            {
                return Result<bool>.ValidationError("ToDate is required.");
            }

            if (reqModel.FromDate > reqModel.ToDate)
            {
                return Result<bool>.ValidationError("FromDate should be earlier than or equal to ToDate.");
            }

            if (reqModel.FromDate < DateOnly.FromDateTime(DateTime.Today))
            {
                return Result<bool>.ValidationError("FromDate cannot be in the past.");
            }

            if (!Enum.IsDefined(typeof(EnumFullOrHalfLeave), reqModel.FullOrHalf))
            {
                return Result<bool>.ValidationError("Leave can only be Full leave or half leave!");
            }

            if (reqModel.FromDate < reqModel.ToDate && reqModel.FullOrHalf == EnumFullOrHalfLeave.HalfLeave)
            {
                return Result<bool>.ValidationError("Half leave can only be applied for a single day.");
            }

            #endregion

            #region Check Leave already taken

            var taken = await _daLeave.ValidateLeaveOverlapAsync(UserCode, reqModel.FromDate, reqModel.ToDate);
            if (!taken.IsSuccess)
                return taken;

            #endregion

            var generatedCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.EMP.ToString());
            var isPaidLeave = reqModel.LeaveType != EnumLeaveType.LeaveWithoutPay;
            var requestedDays = (reqModel.ToDate.DayNumber - reqModel.FromDate.DayNumber) + 1;

            #region Get Working Hour By Half Leave or Full Leave & Calculate Total Leave Hours

            var ruleCode = reqModel.FullOrHalf == EnumFullOrHalfLeave.HalfLeave
                ? RuleCode.HalfWorkingHour
                : RuleCode.FullWorkingHour;

            var workingHourRule = await _daRule.GetRuleByCode(ruleCode);
            if (workingHourRule?.Data is null || workingHourRule.IsError)
            {
                return Result<bool>
                    .SystemError("An error occured while requesting leave. Try requesting later!");
            }

            var totalLeaveHours = reqModel.FullOrHalf == EnumFullOrHalfLeave.HalfLeave
                ? workingHourRule.Data.Value.ToInt()
                : requestedDays * workingHourRule.Data.Value.ToInt();

            #endregion

            var leave = new TblLeave
            {
                LeaveId = DevCode.GenerateNewUlid(),
                LeaveCode = generatedCode,
                EmployeeCode = UserCode,
                FromDate = reqModel.FromDate,
                ToDate = reqModel.ToDate,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = UserCode,
                Reason = reqModel.Reason,
                LeaveType = reqModel.LeaveType.ToString(),
                TotalHours = totalLeaveHours,
                Status = EnumLeaveStatus.Pending.ToString(),
                FullOrHalf = reqModel.FullOrHalf.ToString(),
                IsPaid = isPaidLeave
            };

            var result = await _daLeave.CreateLeave(leave);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString(), "Error requesting leave!");
            return Result<bool>.SystemError("An error occured while requesting leave!");
        }
    }

    public async Task<Result<AvailableLeaveResponseModel>> CheckLeaveTypeAvailable(
        AvailableLeaveRequestModel reqModel)
    {
        try
        {
            #region Employee Info

            var employee = await _daEmployee.GetEmployeeByCode(UserCode);
            if (employee == null || employee.IsError)
            {
                return Result<AvailableLeaveResponseModel>.Error(employee?.Message ?? "Employee not found.");
            }

            #endregion

            #region Calculate service year

            var serviceYear = 0;
            if (employee.Data.StartDate.HasValue)
            {
                var serviceDuration = DateTime.UtcNow.Date - employee.Data.StartDate.Value.Date;
                serviceYear = (int)(serviceDuration.TotalDays / 365);
            }

            #endregion

            #region Total Leave Taken

            var leavesTaken = await _daLeave.LeavesTaken(reqModel.LeaveType);

            #endregion

            switch (reqModel.LeaveType)
            {
                case EnumLeaveType.CasualLeave:
                    return await ValidateCasualLeave(reqModel, leavesTaken);

                case EnumLeaveType.EarnLeave:
                    return await ValidateEarnLeave(reqModel, leavesTaken, serviceYear);

                case EnumLeaveType.MedicalLeave:
                    return await ValidateMedicalLeave(reqModel, leavesTaken);

                case EnumLeaveType.MaternityLeave:
                    return await ValidateMaternityLeave(reqModel, leavesTaken, employee.Data);

                case EnumLeaveType.LeaveWithoutPay:
                    return await ValidateLeaveWithoutPay(reqModel, leavesTaken);

                default:
                    return Result<AvailableLeaveResponseModel>.Error("Validation not implemented for this leave type.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking leave type - {reqModel.LeaveType} availability!");
            return Result<AvailableLeaveResponseModel>.SystemError(
                $"An error occurred while checking leave type - {reqModel.LeaveType} availability.");
        }
    }

    private async Task<Result<AvailableLeaveResponseModel>> ValidateCasualLeave(
        AvailableLeaveRequestModel reqModel, int leavesTaken)
    {
        var leaveRule = await _daRule.GetRuleByCode(RuleCode.TotalCasualLeave);
        var allowedDays = leaveRule.Data.Value.ToInt();

        if (leavesTaken >= allowedDays)
        {
            return Result<AvailableLeaveResponseModel>
                .Error("Your casual leave quota has been reached.");
        }

        /*var requestedDays = (reqModel.ToDate.DayNumber - reqModel.FromDate.DayNumber) + 1;
        if (requestedDays > 3)
        {
            return Result<AvailableLeaveResponseModel>
                .Error("Casual leave cannot exceed 3 consecutive days.");
        }*/

        return Result<AvailableLeaveResponseModel>.Success(new AvailableLeaveResponseModel
        {
            LeaveType = reqModel.LeaveType,
            RemainingLeave = allowedDays - leavesTaken,
            TotalLeavenTaken = leavesTaken,
            LeaveAllowedPerYear = allowedDays
        });
    }

    private async Task<Result<AvailableLeaveResponseModel>> ValidateEarnLeave(
        AvailableLeaveRequestModel reqModel, int leavesTaken, int serviceYear)
    {
        var leaveRule = await _daRule.GetRuleByCode(RuleCode.TotalEarnLeave);
        var allowedDays = leaveRule.Data.Value.ToInt();

        if (serviceYear < 1)
        {
            return Result<AvailableLeaveResponseModel>
                .Error("Earn leave requires at least 1 year of service.");
        }

        if (leavesTaken >= allowedDays)
        {
            return Result<AvailableLeaveResponseModel>
                .Error("Your earn leave quota has been reached.");
        }

        return Result<AvailableLeaveResponseModel>.Success(new AvailableLeaveResponseModel
        {
            LeaveType = reqModel.LeaveType,
            RemainingLeave = allowedDays - leavesTaken,
            TotalLeavenTaken = leavesTaken,
            LeaveAllowedPerYear = allowedDays
        });
    }

    private async Task<Result<AvailableLeaveResponseModel>> ValidateMedicalLeave(
        AvailableLeaveRequestModel reqModel, int leavesTaken)
    {
        var leaveRule = await _daRule.GetRuleByCode(RuleCode.TotalMedicalLeave);
        var allowedDays = leaveRule.Data.Value.ToInt();

        /*if (string.IsNullOrWhiteSpace(reqModel.Reason))
            return Result<AvailableLeaveResponseModel>.Error("Medical leave requires a valid reason/doctor's note.");*/

        if (leavesTaken >= allowedDays)
        {
            return Result<AvailableLeaveResponseModel>
                .Error("Your medical leave quota has been reached.");
        }

        return Result<AvailableLeaveResponseModel>.Success(new AvailableLeaveResponseModel
        {
            LeaveType = reqModel.LeaveType,
            RemainingLeave = allowedDays - leavesTaken,
            TotalLeavenTaken = leavesTaken,
            LeaveAllowedPerYear = allowedDays
        });
    }

    private async Task<Result<AvailableLeaveResponseModel>> ValidateLeaveWithoutPay(
        AvailableLeaveRequestModel reqModel, int leavesTaken)
    {
        var hasPaidLeaveRemaining = await HasPaidLeaveRemaining();
        if (hasPaidLeaveRemaining)
        {
            return Result<AvailableLeaveResponseModel>.Error(
                "Leave Without Pay is only allowed when paid leave is exhausted.");
        }

        return Result<AvailableLeaveResponseModel>.Success(new AvailableLeaveResponseModel
        {
            LeaveType = reqModel.LeaveType,
            RemainingLeave = int.MaxValue, // unlimited
            TotalLeavenTaken = leavesTaken,
            LeaveAllowedPerYear = int.MaxValue
        });
    }

    private async Task<Result<AvailableLeaveResponseModel>> ValidateMaternityLeave(
        AvailableLeaveRequestModel reqModel, int leavesTaken, EmployeeEditResponseModel employee)
    {
        var leaveRule = await _daRule.GetRuleByCode(RuleCode.TotalMaternityLeave);
        var allowedDays = leaveRule.Data.Value.ToInt();

        if (!string.Equals(employee.Gender, "Female", StringComparison.OrdinalIgnoreCase))
            return Result<AvailableLeaveResponseModel>.Error("Maternity leave is only available for female employees.");

        if (leavesTaken >= allowedDays)
            return Result<AvailableLeaveResponseModel>.Error("Your maternity leave quota has been reached.");

        /*if (reqModel.FromDate < DateOnly.FromDateTime(DateTime.Today.AddDays(28)))
            return Result<AvailableLeaveResponseModel>.Error(
                "Maternity leave must be requested at least 4 weeks in advance.");*/

        return Result<AvailableLeaveResponseModel>.Success(new AvailableLeaveResponseModel
        {
            LeaveType = reqModel.LeaveType,
            RemainingLeave = allowedDays - leavesTaken,
            TotalLeavenTaken = leavesTaken,
            LeaveAllowedPerYear = allowedDays
        });
    }

    private async Task<bool> HasPaidLeaveRemaining()
    {
        var paidLeaveTypes = new[]
        {
            EnumLeaveType.MedicalLeave,
            EnumLeaveType.CasualLeave,
            EnumLeaveType.EarnLeave
        };

        foreach (var leaveType in paidLeaveTypes)
        {
            var rule = await _daRule.GetRuleByCode(GetRuleCodeForLeaveType(leaveType));
            var allowedDays = rule.Data.Value.ToInt();

            var leavesTaken = await _daLeave.LeavesTaken(leaveType);

            if (leavesTaken < allowedDays)
            {
                return true;
            }
        }

        return false;
    }

    private string GetRuleCodeForLeaveType(EnumLeaveType leaveType)
    {
        return leaveType switch
        {
            EnumLeaveType.MedicalLeave => RuleCode.TotalMedicalLeave,
            EnumLeaveType.CasualLeave => RuleCode.TotalCasualLeave,
            EnumLeaveType.EarnLeave => RuleCode.TotalEarnLeave,
            EnumLeaveType.MaternityLeave => RuleCode.TotalMaternityLeave,
            _ => throw new ArgumentException("Invalid paid leave type")
        };
    }

    public async Task<Result<bool>> ApproveLeaveAsync(LeaveApproveRequestModel reqModel)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(reqModel.LeaveCode))
            {
                return Result<bool>.ValidationError("Leave not found.");
            }

            var leave = await _daLeave.GetLeaveByCodeAsync(reqModel.LeaveCode);

            if (leave == null)
                return Result<bool>.ValidationError("Leave not found.");

            if (leave.Status == EnumLeaveStatus.Approved.ToString())
                return Result<bool>.ValidationError("Leave is already approved.");

            if (leave.Status == EnumLeaveStatus.Rejected.ToString())
                return Result<bool>.ValidationError("Leave has already been rejected.");

            leave.Status = EnumLeaveStatus.Approved.ToString();
            leave.ModifiedAt = DateTime.UtcNow;
            leave.ModifiedBy = UserCode;

            var saved = await _daLeave.UpdateLeaveAsync(leave);

            return saved
                ? Result<bool>.Success("Leave approved successfully.")
                : Result<bool>.Error("Failed to approve leave.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving leave.");
            return Result<bool>.SystemError("An error occurred while approving leave.");
        }
    }

    public async Task<Result<bool>> RejectLeaveAsync(LeaveRejectRequestModel reqModel)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(reqModel.LeaveCode))
            {
                return Result<bool>.ValidationError("Leave not found.");
            }

            var leave = await _daLeave.GetLeaveByCodeAsync(reqModel.LeaveCode);

            if (leave == null)
                return Result<bool>.ValidationError("Leave not found.");

            if (leave.Status == EnumLeaveStatus.Approved.ToString())
                return Result<bool>.ValidationError("Leave has already been approved.");

            if (leave.Status == EnumLeaveStatus.Rejected.ToString())
                return Result<bool>.ValidationError("Leave is already rejected.");

            leave.Status = EnumLeaveStatus.Rejected.ToString();
            leave.ModifiedAt = DateTime.UtcNow;
            leave.ModifiedBy = UserCode;

            var saved = await _daLeave.UpdateLeaveAsync(leave);

            return saved
                ? Result<bool>.Success("Leave rejected successfully.")
                : Result<bool>.Error("Failed to reject leave.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting leave.");
            return Result<bool>.SystemError("An error occurred while rejecting leave.");
        }
    }
}