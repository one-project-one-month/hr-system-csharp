namespace HRSystem.Csharp.Domain.Models.Location;

public class LocationCreateRequestModel
{
    public string LocationCode { get; set; }

    public string? Name { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Radius { get; set; }
}

public class  LocationUpdateRequestModel
{
    public string? Name { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Radius { get; set; }
}
