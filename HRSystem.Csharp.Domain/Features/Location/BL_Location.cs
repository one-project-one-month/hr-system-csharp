namespace HRSystem.Csharp.Domain.Features.Location;

public class BL_Location
{
    private readonly DA_Location _daProject;

    public BL_Location(DA_Location daProject)
    {
        _daProject = daProject;
    }

    public async Task<Result<bool>> CreateLocation(LocationCreateRequestModel location)
    {
        var result = await _daProject.CreateLocation(location);
        return result;
    }

    public async Task<Result<List<LocationResponseModel>>> GetAllLocations()
    {
        var result = await _daProject.GetAllLocations();
        return result;
    }

    public async Task<Result<LocationResponseModel>> GetLocationByCode(string locationCode)
    {
        var result = await _daProject.GetLocationByCode(locationCode);
        return result;
    }

    public async Task<Result<bool>> UpdateLocation(string locationCode, LocationUpdateRequestModel location)
    {
        var result = await _daProject.UpdateLocation(locationCode, location);
        return result;
    }

    public async Task<Result<bool>> DeleteLocation(string locationCode)
    {
        var result = await _daProject.DeleteLocation(locationCode);
        return result;
    }
}