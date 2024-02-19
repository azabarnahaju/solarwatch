using System.Net;
using System.Runtime.InteropServices.JavaScript;

namespace SolarWatch.Services.SunData;

public class SunDataProvider : ISunDataProvider
{
    private readonly ILogger<SunDataProvider> _logger;
    
    public SunDataProvider(ILogger<SunDataProvider> logger)
    {
        _logger = logger;
    }

    public string GetSunData(double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling Sunrise-Sunset API with url: {}", url);
        
        return client.DownloadString(url);
    }
    
    public string GetSunData(double lat, double lon, DateTime date)
    {
        var dateAsString = date.ToString("yyyy-MM-dd");
        
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={dateAsString}";
    
        using var client = new WebClient();
        
        _logger.LogInformation("Calling Sunrise-Sunset API with url: {}", url);
        
        return client.DownloadString(url);
    }
    
}