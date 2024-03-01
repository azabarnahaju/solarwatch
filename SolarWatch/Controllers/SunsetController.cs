﻿using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Model.Enums;
using SolarWatch.Services;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
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

    public SunsetController(ILogger<SunsetController> logger, ICityDataProvider cityDataProvider, ISunDataProvider sunDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _sunDataProvider = sunDataProvider;
        _jsonProcessor = jsonProcessor;
    }
    
    [HttpGet("GetSunset")]
    public async Task<ActionResult<string>> GetSunset(string cityName)
    {
        try
        {
            var city = await GetCity(cityName);
        
            var sunData = await _sunDataProvider.GetSunData(city.Lat, city.Lon);

            return Ok(_jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunset));
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting sunset data", e);
            return NotFound("Error getting sunset data");
        }
        
    }
    
    [HttpGet("GetSunsetOnDate")]
    public async Task<ActionResult<string>> GetSunsetOnDate(string cityName, DateTime date)
    {
        try
        {
            var city = await GetCity(cityName);
        
            var sunData = await _sunDataProvider.GetSunData(city.Lat, city.Lon, date);
        
            return Ok(_jsonProcessor.ProcessSunJsonResponse(sunData, SunMovement.Sunset));
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting sunset data", e);
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