using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Services.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
    private readonly ILogger<CityController> _logger;
    private readonly ICityRepository _cityRepository;

    public CityController(ILogger<CityController> logger, ICityRepository cityRepository)
    {
        _logger = logger;
        _cityRepository = cityRepository;
    }
    
    [HttpPatch("Update")]
    public async Task<ActionResult> UpdateCity(City city)
    {
        try
        {
            _cityRepository.Update(city);
            return Ok("City updated successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating city data");
            return NotFound("Error updating city data");
        }
    }
    
    [HttpPost("Add")]
    public async Task<ActionResult> AddCity(City city)
    {
        try
        {
            _cityRepository.Add(city);
            return Ok("City added successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding city data");
            return NotFound("Error adding city data");
        }
    }
    
    [HttpDelete("Delete")]
    public async Task<ActionResult> DeleteCity(City city)
    {
        try
        {
            _cityRepository.Delete(city);
            return Ok("City deleted successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting city data");
            return NotFound("Error deleting city data");
        }
    }
}