using SolarWatch.Services.CityData;

namespace SolarWatch.IntegrationTests.TestServices;

public class TestCityDataProvider : ICityDataProvider
{
    public Task<string> GetCity(string cityName)
    {
        return Task.FromResult("");
    }
}