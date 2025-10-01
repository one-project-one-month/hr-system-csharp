namespace HRSystem.Csharp.Domain.Helpers;

public static class Mapper
{
    public static TblLocation Map(this LocationCreateRequestModel location)
    {
        return new TblLocation
        {
            LocationId = Ulid.NewUlid().ToString(),
            LocationCode = location.LocationCode,
            Name = location.Name,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Radius = location.Radius,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "system",
            DeleteFlag = false
        };
    }

    public static LocationResponseModel Map(this TblLocation location)
    {
        return new LocationResponseModel
        {
            LocationCode = location.LocationCode,
            Name = location.Name,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Radius = location.Radius,
            CreatedAt = location.CreatedAt,
            CreatedBy = location.CreatedBy,
            ModifiedAt = location.ModifiedAt,
            ModifiedBy = location.ModifiedBy,
            DeleteFlag = location.DeleteFlag
        };
    }
}
