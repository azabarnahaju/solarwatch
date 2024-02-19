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

    public SunriseController(ILogger<SunriseController> logger, ICityDataProvider cityDataProvider)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
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

        return ProcessSunriseJsonResponse(sunData);
    }

    private City GetCity(string cityName)
    {
        var weatherData = _cityDataProvider.GetCity(cityName);

        return ProcessCityJsonResponse(weatherData);
    }
    
    private static City ProcessCityJsonResponse(string weatherData)
    {
        JsonDocument json = JsonDocument.Parse(weatherData);
        JsonElement firstCity = json.RootElement[0];
        JsonElement name = firstCity.GetProperty("name");
        JsonElement lat = firstCity.GetProperty("lat");
        JsonElement lon = firstCity.GetProperty("lon");

        City city = new City
        {
            Name = name.GetString(),
            Lat = lat.GetDouble(),
            Lon = lon.GetDouble()
        };

        return city;
    }
    
    private static string ProcessSunriseJsonResponse(string sunData)
    {
        JsonDocument json = JsonDocument.Parse(sunData);
        JsonElement results = json.RootElement.GetProperty("results");
        JsonElement time = results.GetProperty("sunrise");

        return time.GetString();
    }
}