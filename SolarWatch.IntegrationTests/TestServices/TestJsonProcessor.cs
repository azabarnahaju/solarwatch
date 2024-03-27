using SolarWatch.Model;
using SolarWatch.Model.Enums;
using SolarWatch.Services.JsonProcessing;

namespace SolarWatch.IntegrationTests.TestServices;

public class TestJsonProcessor : IJsonProcessor
{
    public City ProcessCityJsonResponse(string cityData)
    {
        return new City {Country = "TEST", Lat = 1.23456, Lon = 1.23456, Name = "TestCity", State = ""};
    }

    public string ProcessSunJsonResponse(string sunData, SunMovement sunMovement)
    {
        return "Test_Information";
    }
}