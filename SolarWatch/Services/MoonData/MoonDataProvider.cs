using System.Globalization;
using System.Net.Http.Headers;
using SolarWatch.Utilities;

namespace SolarWatch.Services.MoonData;

public class MoonDataProvider : IMoonDataProvider
{
    private readonly ILogger<MoonDataProvider> _logger;
    private readonly IConfiguration _config;

    public MoonDataProvider(ILogger<MoonDataProvider> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }
    
    public async Task<string> GetMoonData(double lat, double lon, DateTime date)
    {
        var apiKey = _config["ApiKeys_StormGlass"];
        var dateAsString = date.ToString("yyyy-MM-dd");

        var url = $"https://api.stormglass.io/v2/astronomy/point?lat={Converter.ConvertDoubleFormat(lat)}&lng={Converter.ConvertDoubleFormat(lon)}&start={dateAsString}";
 
        using var client = new HttpClient();
        if (!client.DefaultRequestHeaders.Contains("Authorization"))
        {
            client.DefaultRequestHeaders.Add("Authorization", apiKey);
        }
        _logger.LogInformation("Calling Astronomy | MoonData API with url: {}", url);
        
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }

    public string GetMoonDataPlaceholder(double lat, double lon, DateTime date)
    {
        return
            "{\n    \"data\": [\n        {\n            \"astronomicalDawn\": \"2018-11-22T04:29:13+00:00\",\n            \"astronomicalDusk\": \"2018-11-22T16:43:25+00:00\",\n            \"civilDawn\": \"2018-11-22T06:07:58+00:00\",\n            \"civilDusk\": \"2018-11-22T15:04:39+00:00\",\n            \"moonFraction\": 0.9773405348657047,\n            \"moonPhase\": {\n                \"closest\": {\n                    \"text\": \"Full moon\",\n                    \"time\": \"2018-11-23T10:05:00+00:00\",\n                    \"value\": 0.5\n                },\n                \"current\": {\n                    \"text\": \"Waxing gibbous\",\n                    \"time\": \"2018-11-22T00:00:00+00:00\",\n                    \"value\": 0.45190179144442527\n                }\n            },\n            \"moonrise\": \"2018-11-22T13:58:41.948883+00:00\",\n            \"moonset\": \"2018-11-22T05:04:59.690726+00:00\",\n            \"nauticalDawn\": \"2018-11-22T05:17:04+00:00\",\n            \"nauticalDusk\": \"2018-11-22T15:55:34+00:00\",\n            \"sunrise\": \"2018-11-22T06:56:32+00:00\",\n            \"sunset\": \"2018-11-22T14:16:06+00:00\",\n            \"time\": \"2018-11-22T00:00:00+00:00\"\n        }\n   ],\n    \"meta\": {\n        \"cost\": 1,\n        \"dailyQuota\": 50,\n        \"lat\": 58.7984,\n        \"lng\": 17.8081,\n        \"requestCount\": 1,\n        \"start\": \"2018-11-22T00:00:00+00:00\"\n    }\n}";
    }
    
}