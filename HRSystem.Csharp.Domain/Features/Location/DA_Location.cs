namespace HRSystem.Csharp.Domain.Features.Location;

public class DA_Location
{
    private readonly AppDbContext _appDbContext;

    public DA_Location(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Result<Boolean> CreateLocation(LocationCreateRequestModel location)
    {
        var existingLocation = _appDbContext.TblLocations
            .FirstOrDefault(l => l.LocationCode == location.LocationCode && l.DeleteFlag == false);

        if (existingLocation != null)
            return Result<Boolean>.DuplicateRecordError("Location with the same code already exists");

        var newLocation = location.Map();

        _appDbContext.TblLocations.Add(newLocation);
        var result = _appDbContext.SaveChanges();

        return result > 0 ? Result<Boolean>.Success(true)
            : Result<Boolean>.Error("Failed to create location");
    }

    public Result<List<LocationResponseModel>> GetAllLocations()
    {
        var locations = _appDbContext.TblLocations
            .Where(l => l.DeleteFlag == false)
            .Select(l => l.Map())
            .ToList();

        if (locations == null || locations.Count == 0)
        {
            return Result<List<LocationResponseModel>>.NotFoundError("No locations found");
        }
        return Result<List<LocationResponseModel>>.Success(locations);
    }

    public Result<LocationResponseModel> GetLocationByCode(string locationCode)
    {
        var location = _appDbContext.TblLocations
            .Where(l => l.DeleteFlag == false)
            .Select(l => l.Map())
            .FirstOrDefault(l => l.LocationCode == locationCode);

        if (location == null)
        {
            return Result<LocationResponseModel>.NotFoundError("Location not found");
        }
        return Result<LocationResponseModel>.Success(location);
    }

    public Result<Boolean> UpdateLocation(string locationCode, LocationUpdateRequestModel location)
    {
        var existingLocation = _appDbContext.TblLocations
            .FirstOrDefault(l => l.LocationCode == locationCode && l.DeleteFlag == false);

        if (existingLocation == null)
        {
            return Result<Boolean>.NotFoundError("Location not found");
        }

        existingLocation.Name = location.Name ?? existingLocation.Name;
        existingLocation.Latitude = location.Latitude ?? existingLocation.Latitude;
        existingLocation.Longitude = location.Longitude ?? existingLocation.Longitude;
        existingLocation.Radius = location.Radius ?? existingLocation.Radius;
        existingLocation.ModifiedAt = DateTime.UtcNow;
        existingLocation.ModifiedBy = "system";

        _appDbContext.TblLocations.Update(existingLocation);
        var result = _appDbContext.SaveChanges();
        return result > 0 ? Result<Boolean>.Success(true)
            : Result<Boolean>.Error("Failed to update location");
    }

    public Result<Boolean> DeleteLocation(string locationCode)
    {
        var existingLocation = _appDbContext.TblLocations
            .FirstOrDefault(l => l.LocationCode == locationCode && l.DeleteFlag == false);

        if (existingLocation == null)
        {
            return Result<Boolean>.NotFoundError("Location not found");
        }

        existingLocation.DeleteFlag = true;
        existingLocation.ModifiedAt = DateTime.UtcNow;
        existingLocation.ModifiedBy = "system";

        _appDbContext.TblLocations.Update(existingLocation);
        var result = _appDbContext.SaveChanges();
        return result > 0 ? Result<Boolean>.Success(true)
            : Result<Boolean>.Error("Failed to delete location");
    }
}
