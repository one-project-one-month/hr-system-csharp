namespace HRSystem.Csharp.Domain.Models.Location;

public class LocationResponseModel
{
    public string LocationCode { get; set; }

    public string? Name { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Radius { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool? DeleteFlag { get; set; }
}
