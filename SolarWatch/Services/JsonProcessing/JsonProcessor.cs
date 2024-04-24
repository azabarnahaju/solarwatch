using System.Data;
using SolarWatch.Utilities;

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

        try
        {
            if (json.RootElement.TryGetProperty("data", out JsonElement data))
            {
                JsonElement moon = data[0];
                JsonElement moonRise = moon.GetProperty("moonrise");
                JsonElement date = moon.GetProperty("time");
                JsonElement moonSet = moon.GetProperty("moonset");
                JsonElement moonFraction = moon.GetProperty("moonFraction");
                JsonElement moonPhase = moon.GetProperty("moonPhase");
                JsonElement current = moonPhase.GetProperty("current");
                JsonElement currentPhase = current.GetProperty("text");
                JsonElement next = moonPhase.GetProperty("closest");
                JsonElement nextPhase = next.GetProperty("text");
                JsonElement nextPhaseDate = next.GetProperty("time");
                
                Console.WriteLine("converted into date:" + moonRise.GetDateTime());
                Console.WriteLine("converted into string then util class:" + Converter.UtcToDateTime(moonRise.GetString()));
                
                return new MoonData
                {
                    CityId = cityId,
                    Date = Converter.UtcToDateTime(date.GetString()),
                    CurrentPhase = currentPhase.GetString(),
                    NextPhase = nextPhase.GetString(),
                    NextPhaseTime = Converter.UtcToDateTime(nextPhaseDate.GetString()),
                    MoonRise = Converter.UtcToDateTime(moonRise.GetString()),
                    MoonSet = Converter.UtcToDateTime(moonSet.GetString()),
                    MoonFraction = moonFraction.GetDouble()
                };
            }

            JsonElement error = json.RootElement.GetProperty("errors");
            JsonElement errorMessage = error.GetProperty("key");
            throw new DataException(errorMessage.GetString());
            
        }
        catch (DataException dataException)
        {
            Console.WriteLine(dataException.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        
    }
}