namespace SolarWatch.Services.JsonProcessing;

using System.Text.Json;
using Model;
using Model.Enums;

public class JsonProcessor : IJsonProcessor
{
    public City ProcessCityJsonResponse(string cityData)
    {
        JsonDocument json = JsonDocument.Parse(cityData);
        JsonElement firstCity = json.RootElement[0];
        JsonElement name = firstCity.GetProperty("name");
        JsonElement lat = firstCity.GetProperty("lat");
        JsonElement lon = firstCity.GetProperty("lon");
        JsonElement country = firstCity.GetProperty("country");
        JsonElement state;
        var hasState = firstCity.TryGetProperty("state", out state);

        City city = new City
        {
            Name = name.GetString(),
            Lat = lat.GetDouble(),
            Lon = lon.GetDouble(),
            Country = country.GetString(),
            State = hasState ? state.GetString() : ""
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

    public MoonData ProcessMoonJsonResponse(string moonData, int cityId)
    {
        JsonDocument json = JsonDocument.Parse(moonData);
        Console.WriteLine("Parsing Json document...");
        JsonElement data = json.RootElement.GetProperty("data");
        Console.WriteLine("Getting data from document...");
        JsonElement moon = data[0];
        Console.WriteLine("Getting moon object from data...");
        JsonElement moonRise = moon.GetProperty("moonrise");
        Console.WriteLine(moonRise);
        JsonElement moonSet = moon.GetProperty("moonset");
        Console.WriteLine(moonSet);
        JsonElement moonFraction = moon.GetProperty("moonFraction");
        Console.WriteLine(moonFraction);
        JsonElement moonPhase = moon.GetProperty("moonPhase");
        Console.WriteLine("Getting moon phase object...");
        JsonElement current = moonPhase.GetProperty("current");
        Console.WriteLine("Getting current moon phase object...");
        JsonElement currentPhase = current.GetProperty("text");
        Console.WriteLine(currentPhase);
        JsonElement next = moonPhase.GetProperty("closest");
        Console.WriteLine("Getting next moon phase object...");
        JsonElement nextPhase = next.GetProperty("text");
        Console.WriteLine(nextPhase);
        JsonElement nextPhaseDate = next.GetProperty("time");
        Console.WriteLine(nextPhaseDate);
        
        return new MoonData
        {
            CityId = cityId,
            Date = DateTime.Today,
            CurrentPhase = currentPhase.GetString(),
            NextPhase = nextPhase.GetString(),
            NextPhaseTime = nextPhaseDate.GetString(),
            MoonRise = moonRise.GetString(),
            MoonSet = moonSet.GetString(),
            MoonFraction = moonFraction.GetDouble()
        };
    }
}