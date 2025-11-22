namespace HRSystem.Csharp.Domain.Models.AdminDashboard;

public class AttendanceHistogramChart
{
        public string Label { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int HalfDayAbsent { get; set; }
        
}
