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
public class SunriseController : ControllerBase
{
    private readonly ILogger<SunriseController> _logger;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly ISunDataProvider _sunDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISolarMovementRepository _sunriseRepository;
    

    public SunriseController(ILogger<SunriseController> logger, ICityDataProvider cityDataProvider, ISunDataProvider sunDataProvider, IJsonProcessor jsonProcessor, ICityRepository cityRepository, ISunriseRepository sunriseRepository)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _jsonProcessor = jsonProcessor;
        _sunDataProvider = sunDataProvider;
        _cityRepository = cityRepository;
        _sunriseRepository = sunriseRepository;
    }
    
    [HttpGet("GetSunrise"), Authorize(Roles="Admin, User")]
    public async Task<ActionResult<string>> GetSunrise(string cityName)
    {
        try
        {
            var city = await _cityRepository.GetCity(cityName);
            while (city is null)
            {
                var cityFromProvider = await GetCity(cityName);
                await _cityRepository.Add(cityFromProvider);
                city = await _cityRepository.GetCity(cityFromProvider.Name);
            }

            var sunData = await _sunriseRepository.GetByCity(city.Id);
            while (sunData is null)
            {
                var sunDataFromProvider = await _sunDataProvider.GetSunData(city.Lat, city.Lon);
                var sunDataFromProviderFormatted =
                    _jsonProcessor.ProcessSunJsonResponse(sunDataFromProvider, SunMovement.Sunrise);
                var sunriseToAdd = new Sunrise
                {
                    CityId = city.Id,
                    Time = sunDataFromProviderFormatted,
                    Date = DateTime.Today
                };
                await _sunriseRepository.Add(sunriseToAdd);
                
                sunData = await _sunriseRepository.GetByCity(city.Id);
            }
            
            return Ok(sunData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunrise data");
            return NotFound("Error getting sunrise data");
        }
        
    }
    
    [HttpGet("GetSunRiseOnDate"), Authorize(Roles="Admin, User")]
    public async Task<ActionResult<string>> GetSunriseOnDate(string cityName, DateTime date)
    {
        try
        {
            var city = await _cityRepository.GetCity(cityName);
            while (city is null)
            {
                var cityFromProvider = await GetCity(cityName);
                await _cityRepository.Add(cityFromProvider);
                city = await  _cityRepository.GetCity(cityFromProvider.Name);
            }
            
            var sunData = await _sunriseRepository.GetByCityAndDate(city.Id, date);
            if (sunData is null)
            {
                var sunDataFromProvider = await _sunDataProvider.GetSunData(city.Lat, city.Lon, date);
                var sunDataFromProviderFormatted =
                    _jsonProcessor.ProcessSunJsonResponse(sunDataFromProvider, SunMovement.Sunrise);
                var sunriseToAdd = new Sunrise
                {
                    CityId = city.Id,
                    Time = sunDataFromProviderFormatted,
                    Date = date
                };
                await _sunriseRepository.Add(sunriseToAdd);
                
                sunData = await _sunriseRepository.GetByCityAndDate(city.Id, date);
            }
        
            return Ok(sunData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunrise data");
            return NotFound("Error getting sunrise data");
        }
        
    }
    
    [HttpPatch("Update"), Authorize(Roles="Admin")]
    public async Task<ActionResult> UpdateSunrise(Sunrise sunrise)
    {
        try
        {
            await _sunriseRepository.Update(sunrise);
            return Ok("Sunrise updated successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating sunrise data");
            return NotFound("Error updating sunrise data");
        }
    }
    
    [HttpPost("Add"), Authorize(Roles="Admin")]
    public async Task<ActionResult> AddSunrise(Sunrise sunrise)
    {
        try
        {
            await _sunriseRepository.Add(sunrise);
            return Ok("Sunrise added successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding sunrise data");
            return NotFound("Error adding sunrise data");
        }
    }
    
    [HttpDelete("Delete"), Authorize(Roles="Admin")]
    public async Task<ActionResult> DeleteSunrise(int id)
    {
        try
        {
            await _sunriseRepository.Delete(id);
            return Ok("Sunrise deleted successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting sunrise data");
            return NotFound("Error deleting sunrise data");
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
            _logger.LogError(e, "Error getting city data.");
            return null;
        }
    }
    
}