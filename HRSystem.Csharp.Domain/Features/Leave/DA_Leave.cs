using HRSystem.Csharp.Domain.Features.Rule;
using HRSystem.Csharp.Domain.Models.Leave;
namespace HRSystem.Csharp.Domain.Features.Leave;

public class DA_Leave : AuthorizationService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DA_Leave> _logger;
    private readonly DA_Sequence _daSequence;
    private readonly DA_Rule _daRule;

    public DA_Leave(IHttpContextAccessor httpContextAccessor,
        AppDbContext appDbContext, ILogger<DA_Leave> logger, DA_Sequence daSequence,
        DA_Rule daRule) : base(httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _daSequence = daSequence;
        _daRule = daRule;
    }

    public async Task<Result<bool>> CreateLeave(TblLeave leave)
    {
        await _appDbContext.TblLeaves.AddAsync(leave);
        var saved = await _appDbContext.SaveChangesAsync() > 0;

        return saved
            ? Result<bool>.Success("Leave requested successfully!")
            : Result<bool>.Error("Failed to request leave.");
    }

    public async Task<Result<LeaveListResponseModel>> GetAllRequestLeaves(LeaveListRequestModel leave)
    {
        
        var query = _appDbContext.TblLeaves.Where(l => l.DeleteFlag == false && l.Status == "Pending").AsNoTracking();

        if(!string.IsNullOrEmpty(leave.EmployeeCode))
        {
             query = _appDbContext.TblLeaves.Where(l => l.EmployeeCode.ToLower() == leave.EmployeeCode.ToLower()).AsNoTracking();
        }

        if (!string.IsNullOrEmpty(leave.Status))
        {
            query = _appDbContext.TblLeaves.Where(l => l.Status.ToLower() == leave.Status.ToLower()).AsNoTracking();
        }

        if (!string.IsNullOrEmpty(leave.LeaveType))
        {
             query = _appDbContext.TblLeaves.Where(l => l.LeaveType.ToLower() == leave.LeaveType.ToLower()).AsNoTracking();
        }
        var pagedResult = await query.GetPagedResultAsync(leave.PageNo, leave.PageSize);

        var result = new LeaveListResponseModel
        {
            Items = pagedResult.Items?.Select (l =>
                new LeaveResponseModel
                {
                    LeaveCode = l.LeaveCode,
                    LeaveId = l.LeaveId,
                    LeaveType = l.LeaveType,
                    EmployeeCode = l.EmployeeCode,
                    FromDate = l.FromDate,
                    ToDate = l.ToDate,
                    FullOrHalf = l.FullOrHalf,
                    Reason = l.Reason,
                    IsPaid = l.IsPaid,
                    TotalHours = l.TotalHours,
                    Status = l.Status,
                }
                ).ToList(),
            TotalCount = pagedResult.TotalCount,
            PageNo = leave.PageNo,
            PageSize = leave.PageSize
        };
        return Result<LeaveListResponseModel>.Success(result);

    } 

    public async Task<int> LeavesTaken(EnumLeaveType leaveType)
    {
        var leavesTaken = await _appDbContext.TblLeaves
            .Where(l => l.EmployeeCode == UserCode && l.LeaveType == leaveType.ToString())
            .ToListAsync();

        var daysTaken = leavesTaken.Sum(l => (l.ToDate.DayNumber - l.FromDate.DayNumber) + 1);
        return daysTaken;
    }

    public async Task<Result<bool>> ValidateLeaveOverlapAsync(
        string employeeCode,
        DateOnly fromDate,
        DateOnly toDate)
    {
        var existingLeaves = await _appDbContext.TblLeaves
            .AsNoTracking()
            .Where(l => !l.DeleteFlag
                        && l.EmployeeCode == employeeCode
                        && l.Status != EnumLeaveStatus.Rejected.ToString())
            .ToListAsync();

        var overlappingDays = new List<DateOnly>();

        foreach (var leave in existingLeaves)
        {
            if (fromDate <= leave.ToDate && toDate >= leave.FromDate)
            {
                var overlapStart = fromDate > leave.FromDate ? fromDate : leave.FromDate;
                var overlapEnd = toDate < leave.ToDate ? toDate : leave.ToDate;

                for (var d = overlapStart; d <= overlapEnd; d = d.AddDays(1))
                {
                    overlappingDays.Add(d);
                }
            }
        }

        if (overlappingDays.Any())
        {
            var daysText = string.Join(", ", overlappingDays.Select(d => d.ToString("dd-MM-yyyy")));
            return Result<bool>.ValidationError($"Leave has already taken for the chosen dates: {daysText}");
        }

        return Result<bool>.Success();
    }

    public async Task<TblLeave?> GetLeaveByCodeAsync(string leaveCode)
    {
        var leave = await _appDbContext.TblLeaves
            .FirstOrDefaultAsync(l => !l.DeleteFlag && l.LeaveCode == leaveCode);
        return leave;
    }

    public async Task<bool> UpdateLeaveAsync(TblLeave leave)
    {
        _appDbContext.TblLeaves.Update(leave);
        var response = await _appDbContext.SaveChangesAsync() > 0;
        return response;
    }
}