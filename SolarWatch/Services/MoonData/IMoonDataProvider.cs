using System.Runtime.InteropServices.JavaScript;

namespace SolarWatch.Services.MoonData;

public interface IMoonDataProvider
{
    Task<string> GetMoonData(double lat, double lon, DateTime date);
    string GetMoonDataPlaceholder(double lat, double lon, DateTime date);
}