namespace SolarWatch.Services.SunData;

public interface ISunDataProvider
{
    string GetSunData(double lat, double lon);
    string GetSunData(double lat, double lon, DateTime date);
}