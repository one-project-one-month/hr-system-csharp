namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly BL_Location _blLocation;

    public LocationController(BL_Location blLocation)
    {
        _blLocation = blLocation;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLocations()
    {
        var result = await _blLocation.GetAllLocations();

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        return StatusCode(500, result);
    }

    [HttpGet("{locationCode}")]
    public async Task<IActionResult> GetLocationByCode(string locationCode)
    {
        var result = await _blLocation.GetLocationByCode(locationCode);

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        return StatusCode(500, result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] LocationCreateRequestModel location)
    {
        var result = await _blLocation.CreateLocation(location);

        if (result.IsSuccess) return Ok(result);
        if (result.IsDuplicateRecord) return Conflict(result);
        if (result.IsInvalidData) return BadRequest(result);
        return StatusCode(500, result);
    }

    [HttpPut("{locationCode}")]
    public async Task<IActionResult> UpdateLocation(string locationCode, [FromBody] LocationUpdateRequestModel location)
    {
        var result = await _blLocation.UpdateLocation(locationCode, location);

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        if (result.IsInvalidData) return BadRequest(result);
        return StatusCode(500, result);
    }

    [HttpDelete("{locationCode}")]
    public async Task<IActionResult> DeleteLocation(string locationCode)
    {
        var result = await _blLocation.DeleteLocation(locationCode);

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        return StatusCode(500, result);
    }
}