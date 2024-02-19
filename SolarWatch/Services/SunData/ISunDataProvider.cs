namespace SolarWatch.Services.SunData;

public interface ISunDataProvider
{
    string GetSunData(double lat, double lon);
}