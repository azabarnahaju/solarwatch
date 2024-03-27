using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using SolarWatch.IntegrationTests.JwtAuthenticationTest;
using SolarWatch.Model;
using Xunit.Abstractions;

namespace SolarWatch.IntegrationTests.IntegrationTests;

public class BasicTests : IClassFixture<SolarWatchWebApplicationFactory<Program>>
{
    private readonly SolarWatchWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public BasicTests(
        SolarWatchWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task Should_Reject_Unauthenticated_Requests()
    {
        var response = await _client.GetAsync("/Sunrise/GetSunrise");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [InlineData("/Sunrise/GetSunrise?cityName=TestCity")]
    [InlineData("/Sunrise/GetSunriseOnDate?cityName=TestCity&date=2023-05-06")]
    [InlineData("/Sunrise/GetSunriseOnDate?cityName=TestCity")]
    [InlineData("/Sunset/GetSunset?cityName=TestCity")]
    [InlineData("/Sunset/GetSunsetOnDate?cityName=TestCity&date=2023-05-06")]
    [InlineData("/Sunset/GetSunsetOnDate?cityName=TestCity")]
    public async Task Should_Allow_All_RegisteredUsers_Get(string url)
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Allow_Admin_Update_Or_Add_City()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new City { Id = 1, Country = "TEST", Lat = 1.23456, Lon = 1.23456, Name = "TestCity", State = "TestState"};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var responseUpdate = await _client.PatchAsync("/City/Update", content);
        var responseAdd = await _client.PostAsync("/City/Add", content);
        
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);
        responseAdd.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Not_Allow_User_Update_Or_Add_City()
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new City { Id = 1, Country = "TEST", Lat = 1.23456, Lon = 1.23456, Name = "TestCity", State = "TestState"};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var responseUpdate = await _client.PatchAsync("/City/Update", content);
        var responseAdd = await _client.PostAsync("/City/Add", content);
        
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        responseAdd.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Should_Allow_Admin_Update_Or_Add_Sunrise()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunrise { Id = 1, CityId = 1, Date = new DateTime(2024, 5, 5), Time = ""};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var responseUpdate = await _client.PatchAsync("/Sunrise/Update", content);
        var responseAdd = await _client.PostAsync("/Sunrise/Add", content);
        
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);
        responseAdd.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Not_Allow_User_Update_Or_Add_Sunrise()
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunrise { Id = 1, CityId = 1, Date = new DateTime(2024, 5, 5), Time = ""};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var responseUpdate = await _client.PatchAsync("/Sunrise/Update", content);
        var responseAdd = await _client.PostAsync("/Sunrise/Add", content);
        
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        responseAdd.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Should_Allow_Admin_Update_Or_Add_Sunset()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunset { Id = 1, CityId = 1, Date = new DateTime(2024, 5, 5), Time = ""};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var responseUpdate = await _client.PatchAsync("/Sunset/Update", content);
        var responseAdd = await _client.PostAsync("/Sunset/Add", content);
        
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);
        responseAdd.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Not_Allow_User_Update_Or_Add_Sunset()
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunset { Id = 1, CityId = 1, Date = new DateTime(2024, 5, 5), Time = ""};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var responseUpdate = await _client.PatchAsync("/Sunset/Update", content);
        var responseAdd = await _client.PostAsync("/Sunset/Add", content);
        
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        responseAdd.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}