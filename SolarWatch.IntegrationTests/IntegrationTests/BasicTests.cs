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
    
    public BasicTests(SolarWatchWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task Should_Reject_Unauthenticated_Requests()
    {
        var response = await _client.GetAsync("/Sunrise/GetSunrise?cityName=Budapest");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [InlineData("/Sunrise/GetSunrise?cityName=Paris")]
    [InlineData("/Sunrise/GetSunriseOnDate?cityName=Paris&date=2023-04-08")]
    [InlineData("/Sunrise/GetSunriseOnDate?cityName=Paris")]
    [InlineData("/Sunset/GetSunset?cityName=Paris")]
    [InlineData("/Sunset/GetSunsetOnDate?cityName=Paris&date=2023-04-08")]
    [InlineData("/Sunset/GetSunsetOnDate?cityName=Paris")]
    [InlineData("/Sunrise/GetSunrise?cityName=Budapest")]
    [InlineData("/Sunrise/GetSunriseOnDate?cityName=Budapest&date=2023-04-06")]
    [InlineData("/Sunrise/GetSunriseOnDate?cityName=Budapest")]
    [InlineData("/Sunset/GetSunset?cityName=Budapest")]
    [InlineData("/Sunset/GetSunsetOnDate?cityName=Budapest&date=2023-04-06")]
    [InlineData("/Sunset/GetSunsetOnDate?cityName=Budapest")]
    public async Task Should_Allow_All_RegisteredUsers_Get(string url)
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Allow_Admin_Update_City()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new City { Id = 1, Country = "HU", Lat = 47.4979937, Lon = 19.0403594, Name = "Budapest", State = "Pest"};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var response = await _client.PatchAsync("/City/Update", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Allow_Admin_Add_City()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var city = new City { Country = "TEST", Lat = 1.23456, Lon = 1.23456, Name = "Test", State = "Test" };
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/City/Add", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
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
    public async Task Should_Allow_Admin_Update_Sunrise()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunrise { Id = 1, CityId = 1, Date = new DateTime(2024, 4, 6), Time = "4:09 AM"};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var response = await _client.PatchAsync("/Sunrise/Update", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Allow_Admin_Add_Sunrise()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunrise { CityId = 1, Date = new DateTime(2024, 4, 8), Time = "TEST2"};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/Sunrise/Add", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
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
    public async Task Should_Allow_Admin_Update_Sunset()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunset { Id = 1, CityId = 1, Date = new DateTime(2024, 4, 6), Time = "5:22 PM"};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var response = await _client.PatchAsync("/Sunset/Update", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Allow_Admin_Add_Sunset()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new Sunset { CityId = 1, Date = new DateTime(2024, 4, 8), Time = "TEST"};
        var content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/Sunset/Add", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
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

    [Fact]
    public async Task Should_Allow_Admin_Delete_City()
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("Admin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var id = 1;
        var response = await _client.DeleteAsync($"/Sunrise/Delete?id={id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Not_Allow_User_Delete_City()
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var id = 1;
        var response = await _client.DeleteAsync($"/Sunrise/Delete?id={id}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}