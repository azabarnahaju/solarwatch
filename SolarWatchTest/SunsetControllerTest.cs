using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.SunData;

namespace SolarWatchTest;

[TestFixture]
public class SunsetControllerTest
{
    private Mock<ILogger<SunsetController>> _loggerMock;
    private Mock<ISunDataProvider> _sunDataProviderMock;
    private Mock<ICityDataProvider> _cityDataProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private SunsetController _controller;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SunsetController>>();
        _sunDataProviderMock = new Mock<ISunDataProvider>();
        _cityDataProviderMock = new Mock<ICityDataProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _controller =
            new SunsetController(_loggerMock.Object, _cityDataProviderMock.Object, _sunDataProviderMock.Object, _jsonProcessorMock.Object);
    }
    
    [Test]
    public async Task GetSunset_ReturnsNotFoundResultIfCityDataProviderFails()
    {
        // Arrange
        var cityData = "[]";
        _cityDataProviderMock.Setup(x => x.GetCity(It.IsAny<string>()))
            .Throws(new Exception());
        
        // Act
        var result = await _controller.GetSunset("");
        
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
        var result = await _controller.GetSunset("");
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetSunset_ReturnsOkResultIfCityAndSunDataAreValid()
    {
        
        // Act
        var result = await _controller.GetSunset("budapest");
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
    }
}