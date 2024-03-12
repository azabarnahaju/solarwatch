using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SunData;

namespace SolarWatchTest;

public class SunriseControllerTest
{
    private Mock<ILogger<SunriseController>> _loggerMock;
    private Mock<ISunDataProvider> _sunDataProviderMock;
    private Mock<ICityDataProvider> _cityDataProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private Mock<ICityRepository> _cityRepository;
    private SunriseController _controller;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SunriseController>>();
        _sunDataProviderMock = new Mock<ISunDataProvider>();
        _cityDataProviderMock = new Mock<ICityDataProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _cityRepository = new Mock<ICityRepository>();
        _controller =
            new SunriseController(_loggerMock.Object, _cityDataProviderMock.Object, _sunDataProviderMock.Object, _jsonProcessorMock.Object, _cityRepository.Object);
    }
    
    [Test]
    public async Task GetSunset_ReturnsNotFoundResultIfCityDataProviderFails()
    {
        // Arrange
        var cityData = "[]";
        _cityDataProviderMock.Setup(x => x.GetCity(It.IsAny<string>()))
            .Throws(new Exception());
        
        // Act
        var result = await _controller.GetSunrise("");
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetSunset_ReturnsNotFoundResultIfSunDataProviderFails()
    {
        // Arrange
        var sunData = "{}";
        _sunDataProviderMock.Setup(x => x.GetSunData(It.IsAny<double>(), It.IsAny<double>()));

        // Act
        var result = await _controller.GetSunrise("");
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
}