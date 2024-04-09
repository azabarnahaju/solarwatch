namespace SolarWatch.Services.MoonData;

public interface IMoonDataProvider
{
    Task<string> GetMoonData(double lat, double lon);
    string GetMoonDataPlaceholder(double lat, double lon);
}