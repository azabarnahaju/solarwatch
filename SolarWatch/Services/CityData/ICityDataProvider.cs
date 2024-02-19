namespace SolarWatch.Services;

public interface ICityDataProvider
{
    string GetCity(string cityName);
}