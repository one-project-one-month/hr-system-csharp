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

    public async Task<int> LeavesTaken(EnumLeaveType leaveType)
    {
        var leavesTaken = await _appDbContext.TblLeaves
            .Where(l => l.EmployeeCode == UserCode && l.LeaveType == leaveType.ToString())
            .ToListAsync();

        var daysTaken = leavesTaken.Sum(l => (l.ToDate.DayNumber - l.FromDate.DayNumber) + 1);
        return daysTaken;
    }
}