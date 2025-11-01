namespace HRSystem.Csharp.Domain.Features.Location;

public class DA_Location
{
    private readonly AppDbContext _appDbContext;

    public DA_Location(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Result<bool>> CreateLocation(LocationCreateRequestModel location)
    {
        var existingLocation = await _appDbContext.TblLocations
            .FirstOrDefaultAsync(l => l.LocationCode == location.LocationCode && l.DeleteFlag == false);

        if (existingLocation != null)
            return Result<bool>.DuplicateRecordError("Location with the same code already exists");

        var newLocation = location.Map();

        _appDbContext.TblLocations.Add(newLocation);
        var result = _appDbContext.SaveChanges();

        return result > 0
            ? Result<bool>.Success(true, "Successfully added new location.")
            : Result<bool>.Error("Failed to add new location");
    }

    public async Task<Result<List<LocationResponseModel>>> GetAllLocations()
    {
        var locations = await _appDbContext.TblLocations
            .AsNoTracking()
            .Where(l => l.DeleteFlag == false)
            .Select(l => l.Map())
            .ToListAsync();

        if (locations.Count == 0)
        {
            return Result<List<LocationResponseModel>>.NotFoundError("No locations found");
        }

        return Result<List<LocationResponseModel>>.Success(locations);
    }

    public async Task<Result<LocationResponseModel>> GetLocationByCode(string locationCode)
    {
        var location = await _appDbContext.TblLocations
            .AsNoTracking()
            .Where(l => l.DeleteFlag == false)
            .Select(l => l.Map())
            .FirstOrDefaultAsync(l => l.LocationCode == locationCode);

        if (location == null)
        {
            return Result<LocationResponseModel>.NotFoundError("Location not found");
        }

        return Result<LocationResponseModel>.Success(location);
    }

    public async Task<Result<TblLocation>> GetLocationByName(string locationName)
    {
        var location = await _appDbContext.TblLocations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.DeleteFlag == false && l.Name == locationName);

        return location == null
            ? Result<TblLocation>.NotFoundError("Location not found")
            : Result<TblLocation>.Success(location);
    }

    public async Task<Result<bool>> UpdateLocation(string locationCode, LocationUpdateRequestModel location)
    {
        var existingLocation = await _appDbContext.TblLocations
            .FirstOrDefaultAsync(l => l.LocationCode == locationCode && l.DeleteFlag == false);

        if (existingLocation == null)
        {
            return Result<bool>.NotFoundError("Location not found");
        }

        existingLocation.Name = location.Name ?? existingLocation.Name;
        existingLocation.Latitude = location.Latitude ?? existingLocation.Latitude;
        existingLocation.Longitude = location.Longitude ?? existingLocation.Longitude;
        existingLocation.Radius = location.Radius ?? existingLocation.Radius;
        existingLocation.ModifiedAt = DateTime.UtcNow;
        existingLocation.ModifiedBy = "system";

        _appDbContext.TblLocations.Update(existingLocation);
        var result = _appDbContext.SaveChanges();
        return result > 0
            ? Result<bool>.Success(true, "Successfully updated location.")
            : Result<bool>.Error("Failed to update location");
    }

    public async Task<Result<bool>> DeleteLocation(string locationCode)
    {
        var existingLocation = await _appDbContext.TblLocations
            .FirstOrDefaultAsync(l => l.LocationCode == locationCode && l.DeleteFlag == false);

        if (existingLocation == null)
        {
            return Result<bool>.NotFoundError("Location not found");
        }

        existingLocation.DeleteFlag = true;
        existingLocation.ModifiedAt = DateTime.UtcNow;
        existingLocation.ModifiedBy = "system";

        _appDbContext.TblLocations.Update(existingLocation);
        var result = _appDbContext.SaveChanges();
        return result > 0
            ? Result<bool>.Success(true, "Successfully deleted location.")
            : Result<bool>.Error("Failed to delete location");
    }
}