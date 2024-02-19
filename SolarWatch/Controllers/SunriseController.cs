using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;
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
    public string GetSunrise(string cityName)
    {
        var city = GetCity(cityName);
        
        var sunData = _sunDataProvider.GetSunData(city.Lat, city.Lon);

        return _jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunrise);
    }

    private City GetCity(string cityName)
    {
        var cityData = _cityDataProvider.GetCity(cityName);

        return _jsonProcessor.ProcessCityJsonResponse(cityData);
    }
    
}