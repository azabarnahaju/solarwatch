using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Model.Enums;
using SolarWatch.Services;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SunData;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunsetController : ControllerBase
{
    private readonly ILogger<SunsetController> _logger;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly ISunDataProvider _sunDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISolarMovementRepository _sunsetRepository;

    public SunsetController(ILogger<SunsetController> logger, ICityDataProvider cityDataProvider, ISunDataProvider sunDataProvider, IJsonProcessor jsonProcessor, ICityRepository cityRepository, ISunsetRepository sunsetRepository)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _sunDataProvider = sunDataProvider;
        _jsonProcessor = jsonProcessor;
        _cityRepository = cityRepository;
        _sunsetRepository = sunsetRepository;
    }
    
    [HttpGet("GetSunset"), Authorize]
    public async Task<ActionResult<string>> GetSunset(string cityName)
    {
        try
        {
            var city = _cityRepository.GetCity(cityName);
            while (city is null)
            {
                var cityFromProvider = await GetCity(cityName);
                _cityRepository.Add(cityFromProvider);
                city = _cityRepository.GetCity(cityFromProvider.Name);
            }

            var sunData = _sunsetRepository.GetByCity(city.Id);
            while (sunData is null)
            {
                var sunDataFromProvider = await _sunDataProvider.GetSunData(city.Lat, city.Lon);
                var sunDataFromProviderFormatted =
                    _jsonProcessor.ProcessSunJsonResponse(sunDataFromProvider, SunMovement.Sunset);
                var sunsetToAdd = new Sunset
                {
                    CityId = city.Id,
                    Time = sunDataFromProviderFormatted,
                    Date = DateTime.Today
                };
                _sunsetRepository.Add(sunsetToAdd);
                
                sunData = _sunsetRepository.GetByCity(city.Id);
            }
            
            return Ok(sunData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunset data");
            return NotFound("Error getting sunset data");
        }
        
    }
    
    [HttpGet("GetSunsetOnDate"), Authorize]
    public async Task<ActionResult<string>> GetSunsetOnDate(string cityName, DateTime date)
    {
        try
        {
            var city = _cityRepository.GetCity(cityName);
            while (city is null)
            {
                var cityFromProvider = await GetCity(cityName);
                _cityRepository.Add(cityFromProvider);
                city = _cityRepository.GetCity(cityFromProvider.Name);
            }
            
            var sunData = _sunsetRepository.GetByCityAndDate(city.Id, date);
            while (sunData is null)
            {
                var sunDataFromProvider = await _sunDataProvider.GetSunData(city.Lat, city.Lon, date);
                var sunDataFromProviderFormatted =
                    _jsonProcessor.ProcessSunJsonResponse(sunDataFromProvider, SunMovement.Sunset);
                var sunsetToAdd = new Sunset
                {
                    CityId = city.Id,
                    Time = sunDataFromProviderFormatted,
                    Date = date
                };
                _sunsetRepository.Add(sunsetToAdd);
                
                sunData = _sunsetRepository.GetByCityAndDate(city.Id, date);
            }
        
            return Ok(sunData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunset data");
            return NotFound("Error getting sunset data");
        }
        
    }
    
    private async Task<City> GetCity(string cityName)
    {
        try
        {
            var cityData = await _cityDataProvider.GetCity(cityName);
        
            return _jsonProcessor.ProcessCityJsonResponse(cityData);
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting city data.", e);
            return null;
        }
    }
}