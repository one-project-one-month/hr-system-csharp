namespace HRSystem.Csharp.Domain.Features.Location;

public class BL_Location
{
    private readonly DA_Location _daProject;

    public BL_Location(DA_Location daProject)
    {
        _daProject = daProject;
    }

    public Result<bool> CreateLocation(LocationCreateRequestModel location)
    {
        var result = _daProject.CreateLocation(location);
        return result;
    }

    public Result<List<LocationResponseModel>> GetAllLocations()
    {
        var result = _daProject.GetAllLocations();
        return result;
    }

    public Result<LocationResponseModel> GetLocationByCode(string locationCode)
    {
        var result = _daProject.GetLocationByCode(locationCode);
        return result;
    }

    public Result<bool> UpdateLocation(string locationCode, LocationUpdateRequestModel location)
    {
        var result = _daProject.UpdateLocation(locationCode, location);
        return result;
    }

    public Result<bool> DeleteLocation(string locationCode)
    {
        var result = _daProject.DeleteLocation(locationCode);
        return result;
    }
}
