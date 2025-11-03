using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Shared.Enums;

namespace HRSystem.Csharp.Domain.Features.Location;

public class DA_Location
{
    private readonly AppDbContext _appDbContext;
    private readonly DA_Sequence _daSequence;

    public DA_Location(AppDbContext appDbContext, DA_Sequence daSequence)
    {
        _appDbContext = appDbContext;
        _daSequence = daSequence;
    }

    public async Task<Result<bool>> CreateLocation(LocationCreateRequestModel location)
    {
        var generatedCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.LOC.ToString());

        var existingLocation = await _appDbContext.TblLocations
            .FirstOrDefaultAsync(l =>!l.DeleteFlag && l.Name == location.Name);

        if (existingLocation != null)
            return Result<bool>.DuplicateRecordError("Location with the same code already exists");

        var newLocation = location.Map();

        newLocation.LocationCode = generatedCode;
        _appDbContext.TblLocations.Add(newLocation);
        var result = await _appDbContext.SaveChangesAsync();

        return result > 0
            ? Result<bool>.Success(true, "Successfully added new location.")
            : Result<bool>.Error("Failed to add new location");
    }

    public async Task<Result<LocationListResponseModel>> GetAllLocations(LocationListRequestModel reqModel)
    {
        try
        {
            var query = _appDbContext.TblLocations
                .AsQueryable()
                .Where(l => !l.DeleteFlag);

            if (!string.IsNullOrWhiteSpace(reqModel.LocationName))
            {
                query = query.Where(l => l.Name.ToLower() == reqModel.LocationName.ToLower());
            }

            query = query.OrderByDescending(l => l.CreatedAt);

            var locations = query.Select(l => l.Map());

            var pagedResult = await locations.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new LocationListResponseModel()
            {
                Items = pagedResult.Items ?? new List<LocationResponseModel>(),
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<LocationListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<LocationListResponseModel>.SystemError("An error occurred while retrieving locations.");
        }
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

    public async Task<Result<bool>> UpdateLocation(string locationCode,
        LocationUpdateRequestModel location)
    {
        var existingLocation = await _appDbContext.TblLocations
            .FirstOrDefaultAsync(l => !l.DeleteFlag && l.LocationCode == locationCode);

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
        var result = await _appDbContext.SaveChangesAsync();
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
        var result = await _appDbContext.SaveChangesAsync();

        return result > 0
            ? Result<bool>.Success(true, "Successfully deleted location.")
            : Result<bool>.Error("Failed to delete location");
    }
}