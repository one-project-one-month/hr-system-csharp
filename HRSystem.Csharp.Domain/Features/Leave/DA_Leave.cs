using HRSystem.Csharp.Domain.Features.CompanyRule;
using HRSystem.Csharp.Domain.Features.Rule;
using HRSystem.Csharp.Domain.Models.Leave;

namespace HRSystem.Csharp.Domain.Features.Leave;

public class DA_Leave : AuthorizationService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DA_Leave> _logger;
    private readonly DA_Sequence _daSequence;
    private readonly DA_Rule _daRule;
    private readonly DA_CompanyRule _companyRule;
    private readonly DA_Employee _daEmployee;

    public DA_Leave(IHttpContextAccessor httpContextAccessor,
        AppDbContext appDbContext, ILogger<DA_Leave> logger, DA_Sequence daSequence,
        DA_Rule daRule, DA_CompanyRule companyRule, DA_Employee daEmployee) : base(httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _daSequence = daSequence;
        _daRule = daRule;
        _companyRule = companyRule;
        _daEmployee = daEmployee;
    }

    public async Task<Result<EmployeeLeaveListResponseModel>> GetEmployeeLeaveList(
        EmployeeLeaveListRequestModel reqModel)
    {
        var query = _appDbContext.TblLeaves
            .AsNoTracking()
            .Where(l => !l.DeleteFlag && l.EmployeeCode == UserCode);

        if (!string.IsNullOrWhiteSpace(reqModel.LeaveType))
        {
            query = query.Where(l => l.LeaveType.ToLower().Contains(reqModel.LeaveType.ToLower()));
        }

        if (reqModel.FromDate != default && reqModel.ToDate != default)
        {
            query = query.Where(l => l.FromDate <= reqModel.ToDate && l.ToDate >= reqModel.FromDate);
        }
        else if (reqModel.FromDate != default)
        {
            query = query.Where(l => l.ToDate >= reqModel.FromDate);
        }
        else if (reqModel.ToDate != default)
        {
            query = query.Where(l => l.FromDate <= reqModel.ToDate);
        }

        query = query.OrderByDescending(l => l.CreatedAt);

        var leaveList = query.Select(l => new EmployeeLeaveResponseModel()
        {
            LeaveType = l.LeaveType,
            LeaveCode = l.LeaveCode,
            TotalHours = l.TotalHours,
            Status = l.Status,
            FullOrHalf = l.FullOrHalf,
            Reason = l.Reason,
            IsPaid = l.IsPaid,
            FromDate = l.FromDate,
            ToDate = l.ToDate
        });

        var pagedResult = await leaveList.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

        var result = new EmployeeLeaveListResponseModel()
        {
            Items = pagedResult.Items,
            TotalCount = pagedResult.TotalCount,
            PageNo = reqModel.PageNo,
            PageSize = reqModel.PageSize
        };

        return Result<EmployeeLeaveListResponseModel>.Success(result);
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
        var query =
            from l in _appDbContext.TblLeaves.AsNoTracking()
            join e in _appDbContext.TblEmployees.AsNoTracking()
                on l.EmployeeCode equals e.EmployeeCode
            where !l.DeleteFlag
            select new
            {
                Leave = l,
                EmployeeName = e.Name
            };

        // Global search
        if (!string.IsNullOrEmpty(leave.Query))
        {
            var search = leave.Query.Trim();

            query = query.Where(x =>
                EF.Functions.Like(x.EmployeeName, $"%{search}%") ||
                EF.Functions.Like(x.Leave.EmployeeCode, $"%{search}%") ||
                EF.Functions.Like(x.Leave.Status, $"%{search}%")
            );
        }

        // Leave type filter
        if (!string.IsNullOrEmpty(leave.LeaveType))
        {
            query = query.Where(x =>
                x.Leave.LeaveType == leave.LeaveType);
        }

        // Paging defaults
        leave.PageNo = leave.PageNo < 1 ? 1 : leave.PageNo;
        leave.PageSize = leave.PageSize < 1 ? 10 : leave.PageSize;

        var totalCount = await query.CountAsync();

        var pagedResult = await query
            .OrderByDescending(x => x.Leave.FromDate) // optional but recommended
            .Skip((leave.PageNo - 1) * leave.PageSize)
            .Take(leave.PageSize)
            .ToListAsync();

        var result = new LeaveListResponseModel
        {
            Items = pagedResult.Select(l => new LeaveResponseModel
            {
                LeaveCode = l.Leave.LeaveCode,
                LeaveId = l.Leave.LeaveId,
                LeaveType = l.Leave.LeaveType,
                EmployeeCode = l.Leave.EmployeeCode,
                FromDate = l.Leave.FromDate,
                ToDate = l.Leave.ToDate,
                FullOrHalf = l.Leave.FullOrHalf,
                Reason = l.Leave.Reason,
                IsPaid = l.Leave.IsPaid,
                TotalHours = l.Leave.TotalHours,
                Status = l.Leave.Status,
                EmployeeName = l.EmployeeName
            }).ToList(),
            TotalCount = totalCount,
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

    public async Task<Result<bool>> ValidateLeaveOverlapAsync(
        string employeeCode,
        DateOnly fromDate,
        DateOnly toDate,
        string? excludeLeaveCode = null)
    {
        var existingLeaves = await _appDbContext.TblLeaves
            .AsNoTracking()
            .Where(l => !l.DeleteFlag
                        && l.EmployeeCode == employeeCode
                        && l.Status != EnumLeaveStatus.Rejected.ToString()
                        && (excludeLeaveCode == null || l.LeaveCode != excludeLeaveCode))
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
            return Result<bool>.ValidationError($"Leave has already been taken for the chosen dates: {daysText}");
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

    public async Task<List<(string LeaveType, int Count)>> GetLeaveCountsByYearAsync(int year, string employeeCode)
    {
        return await _appDbContext.TblLeaves
            .Where(l => l.EmployeeCode == employeeCode && l.FromDate.Year == year && l.DeleteFlag == false)
            .GroupBy(l => l.LeaveType).Select(g => new ValueTuple<string, int>(g.Key, g.Count())).ToListAsync();
    }
}