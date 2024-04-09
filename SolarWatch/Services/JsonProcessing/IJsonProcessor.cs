namespace SolarWatch.Services.JsonProcessing;

using Model;
using Model.Enums;

public interface IJsonProcessor
{
    City ProcessCityJsonResponse(string cityData);

    string ProcessSunJsonResponse(string sunData, SunMovement sunMovement);

    MoonData ProcessMoonJsonResponse(string moonData, int cityId);
}