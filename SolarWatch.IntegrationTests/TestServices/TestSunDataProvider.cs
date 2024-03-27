using SolarWatch.Services.SunData;

namespace SolarWatch.IntegrationTests.TestServices;

public class TestSunDataProvider : ISunDataProvider
{
    public Task<string> GetSunData(double lat, double lon)
    {
        return Task.FromResult("");
    }

    public Task<string> GetSunData(double lat, double lon, DateTime date)
    {
        return Task.FromResult("");
    }
}