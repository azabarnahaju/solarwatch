using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.MoonData;
using SolarWatch.Services.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class MoonController : ControllerBase
{
    private readonly ILogger<MoonController> _logger;
    private readonly IMoonRepository _moonRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly IMoonDataProvider _moonDataProvider;
    private readonly ICityDataProvider _cityDataProvider;

    public MoonController(ILogger<MoonController> logger, IMoonRepository moonRepository, ICityRepository cityRepository, IJsonProcessor jsonProcessor, IMoonDataProvider moonDataProvider, ICityDataProvider cityDataProvider)
    {
        _logger = logger;
        _moonRepository = moonRepository;
        _cityRepository = cityRepository;
        _jsonProcessor = jsonProcessor;
        _moonDataProvider = moonDataProvider;
        _cityDataProvider = cityDataProvider;
    }

    [HttpGet("GetAll"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetAllMoonData()
    {
        try
        {
            var allMoonData = await _moonRepository.GetAll();
            return Ok(allMoonData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting moon data.");
            return NotFound("Error getting moon data.");
        }
    }
    
    [HttpGet("GetMoonData")]
    public async Task<ActionResult> GetMoonDataByCity(string cityName)
    {
        try
        {
            var city = await _cityRepository.GetCity(cityName);
            while (city is null)
            {
                var cityFromProvider = await GetCity(cityName);
                await _cityRepository.Add(cityFromProvider);
                city = await _cityRepository.GetCity(cityFromProvider.Name);
            }

            var moonData = await _moonRepository.GetByCity(city.Id);
            while (moonData is null)
            {
                var moonDataFromProvider = _moonDataProvider.GetMoonDataPlaceholder(city.Lat, city.Lon);
                _logger.LogInformation(moonDataFromProvider);
                var moonDataToAdd =
                    _jsonProcessor.ProcessMoonJsonResponse(moonDataFromProvider, city.Id);
               
                await _moonRepository.Add(moonDataToAdd);
            
                moonData = await _moonRepository.GetByCity(city.Id);
            }
        
            return Ok(moonData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting moon data.");
            return NotFound("Error getting moon data.");
        }
    }

    [HttpPost("Add"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddMoonData(MoonData moonData)
    {
        try
        {
            await _moonRepository.Add(moonData);
            return Ok("Successfully added moon data.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error while adding moon data.");
            return BadRequest("Error while adding moon data.");
        }
    }
    
    [HttpPatch("Update"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateMoonData(MoonData moonData)
    {
        try
        {
            await _moonRepository.Update(moonData);
            return Ok("Successfully updated moon data.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error while updating moon data.");
            return BadRequest("Error while updating moon data.");
        }
    }
    
    [HttpDelete("Delete"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteMoonData(int id)
    {
        try
        {
            await _moonRepository.Delete(id);
            return Ok("Successfully deleted moon data.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error while deleting moon data.");
            return BadRequest("Error while deleting moon data.");
        }
    }
        
    private async Task<City> GetCity(string cityName)
    {
        try
        {
            var cityData = await _cityDataProvider.GetCity(cityName);
            return _jsonProcessor.ProcessCityJsonResponse(cityData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting city data.");
            return null;
        }
    }
}