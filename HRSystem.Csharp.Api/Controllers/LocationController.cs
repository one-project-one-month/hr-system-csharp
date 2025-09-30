using HRSystem.Csharp.Domain.Features.Location;
using HRSystem.Csharp.Domain.Models.Location;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAllLocations()
    {
        var result = _blLocation.GetAllLocations();

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        return StatusCode(500, result);
    }

    [HttpGet("{locationCode}")]
    public IActionResult GetLocationByCode(string locationCode)
    {
        var result = _blLocation.GetLocationByCode(locationCode);

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        return StatusCode(500, result);
    }

    [HttpPost]
    public IActionResult CreateLocation([FromBody] LocationCreateRequestModel location)
    {
        var result = _blLocation.CreateLocation(location);

        if (result.IsSuccess) return Ok(result);
        if (result.IsDuplicateRecord) return Conflict(result);
        if (result.IsInvalidData) return BadRequest(result);
        return StatusCode(500, result);
    }

    [HttpPut("{locationCode}")]
    public IActionResult UpdateLocation(string locationCode, [FromBody] LocationUpdateRequestModel location)
    {
        var result = _blLocation.UpdateLocation(locationCode, location);

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        if (result.IsInvalidData) return BadRequest(result);
        return StatusCode(500, result);
    }

    [HttpDelete("{locationCode}")]
    public IActionResult DeleteLocation(string locationCode)
    {
        var result = _blLocation.DeleteLocation(locationCode);

        if (result.IsSuccess) return Ok(result);
        if (result.IsNotFound) return NotFound(result);
        return StatusCode(500, result);
    }
}