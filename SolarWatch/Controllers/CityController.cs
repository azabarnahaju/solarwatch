using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
    private readonly ILogger<CityController> _logger;

    public CityController(ILogger<CityController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetCityLatLon")]
    public City GetCity(string cityName)
    {
        var apiKey = "d80b2959da1f5d7225828323dee566bd";
        
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling OpenWeather API with url: {}", url);
        var weatherData = client.DownloadString(url);

        return ProcessJsonResponse(weatherData);
    }

    private static City ProcessJsonResponse(string weatherData)
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
}