namespace SolarWatch.Services.CityData;

public interface ICityDataProvider
{
    string GetCity(string cityName);
}