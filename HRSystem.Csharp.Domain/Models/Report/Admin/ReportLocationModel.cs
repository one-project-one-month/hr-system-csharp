namespace HRSystem.Csharp.Domain.Models.Report.Admin;

public class ReportLocationModel
{
    public string LocationName { get; set; } = null!;
    public string Latitude { get; set; } = null!;
    public string Longitude { get; set; } = null!;
    public string Radius { get; set; } = null!;
}