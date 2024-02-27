using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.SunData;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseController : ControllerBase
{
    private readonly ILogger<SunriseController> _logger;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly ISunDataProvider _sunDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    

    public SunriseController(ILogger<SunriseController> logger, ICityDataProvider cityDataProvider, ISunDataProvider sunDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _jsonProcessor = jsonProcessor;
        _sunDataProvider = sunDataProvider;
    }
    
    [HttpGet("GetSunrise")]
    public async Task<ActionResult<string>> GetSunrise(string cityName)
    {
        try
        {
            var city = await GetCity(cityName);
            
            var sunData = await _sunDataProvider.GetSunData(city.Lat, city.Lon);

            return Ok(_jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunrise));
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting sunrise data", e);
            return NotFound("Error getting sunrise data");
        }
        
    }
    
    [HttpGet("GetSunRiseOnDate")]
    public async Task<ActionResult<string>> GetSunriseOnDate(string cityName, DateTime date)
    {
        try
        {
            var city = await GetCity(cityName);
        
            var sunData = await _sunDataProvider.GetSunData(city.Lat, city.Lon, date);
        
            return Ok(_jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunrise));
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting sunrise data", e);
            return NotFound("Error getting sunrise data");
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
            _logger.LogError("Error getting city data.", e);
            return null;
        }
    }
    
}