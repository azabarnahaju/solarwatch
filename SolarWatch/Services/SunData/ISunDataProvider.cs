namespace SolarWatch.Services.SunData;

public interface ISunDataProvider
{
    Task<string> GetSunData(double lat, double lon);
    Task<string> GetSunData(double lat, double lon, DateTime date);
}