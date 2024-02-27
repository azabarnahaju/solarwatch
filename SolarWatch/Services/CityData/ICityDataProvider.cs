namespace SolarWatch.Services.CityData;

public interface ICityDataProvider
{
    Task<string> GetCity(string cityName);
}