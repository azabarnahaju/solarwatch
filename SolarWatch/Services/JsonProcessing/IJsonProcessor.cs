namespace SolarWatch.Services.JsonProcessing;

public interface IJsonProcessor
{
    City ProcessCityJsonResponse(string cityData);

    string ProcessSunJsonResponse(string sunData, SunMovement sunMovement);

}