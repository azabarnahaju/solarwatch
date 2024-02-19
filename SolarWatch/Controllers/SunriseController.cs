using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseController : ControllerBase
{
    private readonly ILogger<SunriseController> _logger;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SunriseController(ILogger<SunriseController> logger, ICityDataProvider cityDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _jsonProcessor = jsonProcessor;
    }
    
    [HttpGet("GetSunrise")]
    public string GetSunrise(string cityName)
    {
        var city = GetCity(cityName);
        var lat = city.Lat;
        var lon = city.Lon;
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling Sunrise-Sunset API with url: {}", url);
        var sunData = client.DownloadString(url);

        return _jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunrise);
    }

    private City GetCity(string cityName)
    {
        var cityData = _cityDataProvider.GetCity(cityName);

        return _jsonProcessor.ProcessCityJsonResponse(cityData);
    }
    
}