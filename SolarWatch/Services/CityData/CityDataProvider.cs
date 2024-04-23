using System.Net;

namespace SolarWatch.Services.CityData;

public class CityDataProvider : ICityDataProvider
{
    private readonly ILogger<CityDataProvider> _logger;
    private readonly IConfiguration _config;

    public CityDataProvider(ILogger<CityDataProvider> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public async Task<string> GetCity(string cityName)
    {
        var apiKey = _config["ApiKeys:OpenWeatherAPI"];
        
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";

        using var client = new HttpClient();
        _logger.LogInformation("Calling OpenWeather API with url: {}", url);
        
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}