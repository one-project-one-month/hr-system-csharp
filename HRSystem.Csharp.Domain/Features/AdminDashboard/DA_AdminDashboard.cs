using HRSystem.Csharp.Domain.Models.AdminDashboard;

namespace HRSystem.Csharp.Domain.Features.AdminDashboard;

public class DA_AdminDashboard
{
        private readonly AppDbContext _context;

        public DA_AdminDashboard(AppDbContext context)
        {
                _context = context;
        }

        public async Task<Result<StatsCard>> GetStatsCards()
        {
                try
                {
                        var employees = await _context.TblEmployees.Where(e => !e.DeleteFlag).ToListAsync();

                        int employeeCount = employees.Count();

                        var attendedToday = await _context.TblAttendances
                                .Where(a => !a.DeleteFlag &&
                                a.AttendanceDate.HasValue && 
                                a.AttendanceDate.Value.Date == DateTime.Today)
                                .Select(a => a.EmployeeCode)
                                .ToListAsync();

                        int absentCount = employees.Where(e => !attendedToday.Contains(e.EmployeeCode)).Count();

                        int projectCount = await _context.TblProjects.Where(p => !p.DeleteFlag).CountAsync();

                        var statsCard = new StatsCard
                        {
                                TotalEmployee = employeeCount,
                                TodayAbsence = absentCount,
                                TotalProject = projectCount,
                        };

                        return Result<StatsCard>.Success(statsCard);
                }
                catch (Exception ex)
                {
                        return Result<StatsCard>.Error($"Error occured while retrieving stats cards: {ex.Message}");
                }
        }
}
