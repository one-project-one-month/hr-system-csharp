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
                        var employees = await _context.TblEmployees.AsNoTracking()
                                .Where(e => !e.DeleteFlag).ToListAsync();

                        int employeeCount = employees.Count();

                        var attendedToday = await _context.TblAttendances
                                .AsNoTracking()
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

        public async Task<Result<List<AttendanceHistogramChart>>> GetAttendanceSummary(string type)
        {
                try
                {
                        var employees = await _context.TblEmployees.AsNoTracking()
                                .Where(e => !e.DeleteFlag).ToListAsync();

                        if (employees.Count == 0) return Result<List<AttendanceHistogramChart>>
                                        .NotFoundError("No employees data found!");

                        if (type.Equals("weekly"))
                                return await GetWeeklyAttendanceSummary(employees);
                        

                        if (type.Equals("monthly"))
                                return await GetMonthlyAttendanceSummary(employees);

                        return Result<List<AttendanceHistogramChart>>
                                        .BadRequestError("parameter must be 'weekly' or 'monthly'.");
                        
                }
                catch (Exception ex)
                {
                        return Result<List<AttendanceHistogramChart>>
                                .Error($"Error occured while retrieving stats cards: {ex.Message}");
                }
        }

        private async Task<Result<List<AttendanceHistogramChart>>> GetWeeklyAttendanceSummary(List<TblEmployee> employees)
        {
                var latestDay = await _context.TblAttendances.Where(a => !a.DeleteFlag)
                        .OrderByDescending(a => a.AttendanceDate)
                        .Select(a => a.AttendanceDate)
                        .FirstOrDefaultAsync();
                
                
                if (latestDay is null) return Result<List<AttendanceHistogramChart>>
                                .NotFoundError("No attendances data found!");

                var end = latestDay.Value.Date;
                var start = end.AddDays(-4);

                var attendances = await _context.TblAttendances
                        .AsNoTracking()
                        .Where(a => !a.DeleteFlag && a.AttendanceDate >= start &&
                        a.AttendanceDate <= end)
                        .ToListAsync();

                var data = attendances
                        .GroupBy(a => a.AttendanceDate!.Value.Date)
                        .OrderBy(a => a.Key)
                        .Select(a =>
                        {
                                var attendedEmployees = a.Select(x => x.EmployeeCode);
                                return new AttendanceHistogramChart
                                {
                                        Label = a.Key.ToString("MMM d"),
                                        Present = a.Count(a => a.HalfDayFlag!.Value == 0 && a.FullDayFlag!.Value == 0),
                                        Absent = employees.Where(e => !attendedEmployees.Contains(e.EmployeeCode)).Count(),
                                        HalfDayAbsent = a.Count(a => a.HalfDayFlag!.Value == 1)
                                };
                        })
                        .ToList();
                return Result<List<AttendanceHistogramChart>>.Success(data);
        }

        private async Task<Result<List<AttendanceHistogramChart>>> GetMonthlyAttendanceSummary(List<TblEmployee> employees)
        {
                int year = DateTime.Now.Year;

                var attendances = await _context.TblAttendances
                        .AsNoTracking()
                        .Where(a => !a.DeleteFlag && a.AttendanceDate!.Value.Year == year)
                        .ToListAsync();

                var data = attendances
                        .GroupBy(a => new
                        {
                                Year = a.AttendanceDate!.Value.Year,
                                Month = a.AttendanceDate!.Value.Month
                        })
                        .OrderBy(g => g.Key.Month)
                        .Select(g =>
                        {
                                var attendedEmployees = g.Select(x => x.EmployeeCode);
                                return new AttendanceHistogramChart
                                {
                                        Label = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                                        Present = g.Count(x => x.HalfDayFlag == 0 && x.FullDayFlag == 0),
                                        Absent = employees.Count(e => !attendedEmployees.Contains(e.EmployeeCode)),
                                        HalfDayAbsent = g.Count(x => x.HalfDayFlag == 1)
                                };
                        })
                        .ToList();

                return Result<List<AttendanceHistogramChart>>.Success(data);
        }
}
