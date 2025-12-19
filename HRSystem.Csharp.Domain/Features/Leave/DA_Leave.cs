using HRSystem.Csharp.Domain.Models.Leave;

namespace HRSystem.Csharp.Domain.Features.Leave;

public class DA_Leave : AuthorizationService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DA_Leave> _logger;
    private readonly DA_Sequence _daSequence;

    public DA_Leave(HttpContextAccessor httpContextAccessor,
        AppDbContext appDbContext, ILogger<DA_Leave> logger, DA_Sequence daSequence) : base(httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _daSequence = daSequence;
    }

    public async Task<Result<bool>> CreateLeave(LeaveCreateRequestModel reqModel)
    {
        var generatedCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.EMP.ToString());

        var leave = new TblLeave()
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
            TotalHours = reqModel.TotalHours,
            Status = EnumLeaveStatus.Pending.ToString(),
            FullOrHalf = reqModel.FullOrHalf.ToString(),
            IsPaid = reqModel.IsPaid
        };

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