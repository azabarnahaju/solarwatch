using System.Net;

namespace SolarWatch.Services.CityData;

public class CityDataProvider : ICityDataProvider
{
    private readonly ILogger<CityDataProvider> _logger;

    public CityDataProvider(ILogger<CityDataProvider> logger)
    {
        _logger = logger;
    }

    public string GetCity(string cityName)
    {
        var apiKey = "d80b2959da1f5d7225828323dee566bd";
        
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling OpenWeather API with url: {}", url);
        return client.DownloadString(url);
    }
}