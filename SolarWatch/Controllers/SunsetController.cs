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
public class SunsetController : ControllerBase
{
    private readonly ILogger<SunsetController> _logger;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly ISunDataProvider _sunDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SunsetController(ILogger<SunsetController> logger, ICityDataProvider cityDataProvider, ISunDataProvider sunDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _sunDataProvider = sunDataProvider;
        _jsonProcessor = jsonProcessor;
    }
    
    [HttpGet("GetSunset")]
    public string GetSunset(string cityName)
    {
        var city = GetCity(cityName);
        
        var sunData = _sunDataProvider.GetSunData(city.Lat, city.Lon);

        return _jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunset);
    }
    
    [HttpGet("GetSunsetOnDate")]
    public string GetSunsetOnDate(string cityName, DateTime date)
    {
        var city = GetCity(cityName);
        
        var sunData = _sunDataProvider.GetSunData(city.Lat, city.Lon, date);
        
        return _jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunrise);
    }
    
    private City GetCity(string cityName)
    {
        var cityData = _cityDataProvider.GetCity(cityName);

        return _jsonProcessor.ProcessCityJsonResponse(cityData);
    }
}