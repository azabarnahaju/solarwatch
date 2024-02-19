using SolarWatch.Controllers;

namespace SolarWatch.Services;

public interface IJsonProcessor
{
    City ProcessCityJsonResponse(string cityData);

    string ProcessSunJsonResponse(string sunData, SunMovement sunMovement);

}