using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Model;
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
    private Mock<ICityRepository> _cityRepositoryMock;
    private SunriseController _controller;
    private Mock<ISunriseRepository> _sunriseRepositoryMock;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SunriseController>>();
        _sunDataProviderMock = new Mock<ISunDataProvider>();
        _cityDataProviderMock = new Mock<ICityDataProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _sunriseRepositoryMock = new Mock<ISunriseRepository>();
        _controller =
            new SunriseController(_loggerMock.Object, _cityDataProviderMock.Object, _sunDataProviderMock.Object, _jsonProcessorMock.Object, _cityRepositoryMock.Object, _sunriseRepositoryMock.Object);
    }
    
    [Test]
    public async Task GetSunrise_ReturnsNotFoundResultIfCityDataProviderFails()
    {
        // Arrange
        _cityDataProviderMock.Setup(x => x.GetCity(It.IsAny<string>()))
            .Throws(new Exception());
        
        // Act
        var result = await _controller.GetSunrise("");
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetSunrise_ReturnsNotFoundResultIfSunDataProviderFails()
    {
        // Arrange
        _sunDataProviderMock.Setup(x => x.GetSunData(It.IsAny<double>(), It.IsAny<double>()));

        // Act
        var result = await _controller.GetSunrise("");
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetSunrise_ReturnsOkResultIfCityAndSunDataAreValid()
    {
        // Arrange
        _cityRepositoryMock.Setup(x => x.GetCity(It.IsAny<string>())).ReturnsAsync(new City());
        _sunriseRepositoryMock.Setup(x => x.GetByCity(It.IsAny<int>())).ReturnsAsync(new Sunrise());
        _sunDataProviderMock.Setup(x => x.GetSunData(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync("");
        
        // Act
        var result = await _controller.GetSunrise("TestCity");
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
    }
}