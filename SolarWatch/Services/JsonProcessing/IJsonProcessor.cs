using SolarWatch.Model;
using SolarWatch.Model.Enums;

namespace SolarWatch.Services.JsonProcessing;

public interface IJsonProcessor
{
    City ProcessCityJsonResponse(string cityData);

    string ProcessSunJsonResponse(string sunData, SunMovement sunMovement);

}