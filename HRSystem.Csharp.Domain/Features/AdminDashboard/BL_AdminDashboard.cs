using HRSystem.Csharp.Domain.Models.AdminDashboard;

namespace HRSystem.Csharp.Domain.Features.AdminDashboard;

public class BL_AdminDashboard
{
        private readonly DA_AdminDashboard _daAdminDashboard;

        public BL_AdminDashboard(DA_AdminDashboard daAdminDashboard)
        {
                _daAdminDashboard = daAdminDashboard;
        }

        public async Task<Result<StatsCard>> GetStatsCards()
        {
                return await _daAdminDashboard.GetStatsCards();
        }

        public async Task<Result<List<AttendanceHistogramChart>>> GetAttendanceHistogram(string type)
        {
                return await _daAdminDashboard.GetAttendanceSummary(type);
        }
}
