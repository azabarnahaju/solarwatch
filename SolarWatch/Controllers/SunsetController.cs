using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunsetController : ControllerBase
{
    private readonly ILogger<SunsetController> _logger;
    private readonly ICityDataProvider _cityDataProvider;

    public SunsetController(ILogger<SunsetController> logger, ICityDataProvider cityDataProvider)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
    }
    
    [HttpGet("GetSunset")]
    public string GetSunrise(string cityName)
    {
        var city = GetCity(cityName);
        var lat = city.Lat;
        var lon = city.Lon;
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling Sunrise-Sunset API with url: {}", url);
        var sunData = client.DownloadString(url);

        return ProcessSunsetJsonResponse(sunData);
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
    
    private static string ProcessSunsetJsonResponse(string sunData)
    {
        JsonDocument json = JsonDocument.Parse(sunData);
        JsonElement results = json.RootElement.GetProperty("results");
        JsonElement time = results.GetProperty("sunset");

        return time.GetString();
    }
}