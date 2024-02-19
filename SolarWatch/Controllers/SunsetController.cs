using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunsetController : ControllerBase
{
    private readonly ILogger<SunsetController> _logger;

    public SunsetController(ILogger<SunsetController> logger)
    {
        _logger = logger;
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
        var apiKey = "d80b2959da1f5d7225828323dee566bd";
        
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling OpenWeather API with url: {}", url);
        var weatherData = client.DownloadString(url);

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