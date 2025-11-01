namespace HRSystem.Csharp.Domain.Features.Location;

public class BL_Location
{
    private readonly DA_Location _daLocation;

    public BL_Location(DA_Location daLocation)
    {
        _daLocation = daLocation;
    }

    public async Task<Result<bool>> CreateLocation(LocationCreateRequestModel location)
    {
        var validationResult = RequestValidator.ValidateCreateLocation(location);

        if (!validationResult.IsSuccess)
            return Result<bool>.BadRequestError(validationResult.Message!);

        var existing = await _daLocation.GetLocationByName(location.Name);
        if (existing is { IsSuccess: true, Data: not null })
        {
            return Result<bool>.DuplicateRecordError("Location name already exists!");
        }

        var result = await _daLocation.CreateLocation(location);
        return result;
    }

    public async Task<Result<List<LocationResponseModel>>> GetAllLocations()
    {
        var result = await _daLocation.GetAllLocations();
        return result;
    }

    public async Task<Result<LocationResponseModel>> GetLocationByCode(string locationCode)
    {
        var result = await _daLocation.GetLocationByCode(locationCode);
        return result;
    }

    public async Task<Result<bool>> UpdateLocation(string locationCode, LocationUpdateRequestModel location)
    {
        var validationResult = RequestValidator.ValidateUpdateLocation(locationCode, location);

        if (!validationResult.IsSuccess)
            return Result<bool>.BadRequestError(validationResult.Message!);

        var result = await _daLocation.UpdateLocation(locationCode, location);
        return result;
    }

    public async Task<Result<bool>> DeleteLocation(string locationCode)
    {
        var result = await _daLocation.DeleteLocation(locationCode);
        return result;
    }
}