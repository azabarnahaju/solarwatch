using System.Text.Json;

namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public City ProcessCityJsonResponse(string cityData)
    {
        JsonDocument json = JsonDocument.Parse(cityData);
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

    public string ProcessSunJsonResponse(string sunData, SunMovement sunMovement)
    {
        JsonDocument json = JsonDocument.Parse(sunData);
        JsonElement results = json.RootElement.GetProperty("results");
        JsonElement time = sunMovement == SunMovement.Sunrise ? results.GetProperty("sunrise") : results.GetProperty("sunset");

        return time.GetString();
    }
    
}