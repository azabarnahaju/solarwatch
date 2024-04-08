using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SolarWatch.Data;
using SolarWatch.IntegrationTests.JwtAuthenticationTest;
using SolarWatch.Model;
using SolarWatch.Services.Authentication;

namespace SolarWatch.IntegrationTests;

public class SolarWatchWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "test");
        builder.ConfigureServices(services =>
        {
            // databases
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));
            var userContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UsersContext>));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);
            if (userContextDescriptor != null) services.Remove(userContextDescriptor);

            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase("SolarWatch_Test");
            });
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseInMemoryDatabase("SolarWatch_Users_Test");
            });
            
            // JWT authorization
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Configuration = new OpenIdConnectConfiguration
                    {
                        Issuer = JwtTokenProvider.Issuer,
                    };
                    options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                    options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                    options.TokenValidationParameters.IssuerSigningKey = JwtTokenProvider.SecurityKey;
                }
            );
            
            // other services
            var authenticationSeederDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(AuthenticationSeeder));
            if (authenticationSeederDescriptor != null) services.Remove(authenticationSeederDescriptor);
            services.AddScoped<AuthenticationSeeder>(provider =>
            {
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
                
                var testAdminInfo = new Dictionary<string, string>
                {
                    {"adminEmail", "test@admin.com"},
                    {"adminPassword", "testAdminPassword"}
                };
                return new AuthenticationSeeder(roleManager, userManager, testAdminInfo);
            });
            
            var tokenServiceDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(ITokenService));
            if (tokenServiceDescriptor != null) services.Remove(tokenServiceDescriptor);
            services.AddScoped<ITokenService>(provider => new TokenService(JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer, JwtTokenProvider.IssuerSigningKey));

            SeedTestData(services);
            
            // // adding mock repositories
            // var cityRepositoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICityRepository));
            // if (cityRepositoryDescriptor != null)
            //     services.Remove(cityRepositoryDescriptor);
            // var sunriseRepositoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISunriseRepository));
            // if (sunriseRepositoryDescriptor != null)
            //     services.Remove(sunriseRepositoryDescriptor);
            // var sunsetRepositoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISunsetRepository));
            // if (sunsetRepositoryDescriptor != null)
            //     services.Remove(sunsetRepositoryDescriptor);
            //
            // // Create mock repository instances
            // var mockCityRepository = new Mock<ICityRepository>();
            // var mockSunsetRepository = new Mock<ISunsetRepository>();
            // var mockSunriseRepository = new Mock<ISunriseRepository>();
            //
            // // CITY
            // mockCityRepository.Setup(repo => repo.GetCity(It.IsAny<string>()))
            //     .ReturnsAsync((string cityName) => new City { /* mocked city object */ });
            // mockCityRepository.Setup(repo => repo.Add(It.IsAny<City>()))
            //     .Returns(Task.CompletedTask);
            // mockCityRepository.Setup(repo => repo.Update(It.IsAny<City>()))
            //     .Returns(Task.CompletedTask);
            // mockCityRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
            //     .Returns(Task.CompletedTask);
            //
            // // SUNSET
            // mockSunsetRepository.Setup(repo => repo.GetByCity(It.IsAny<int>()))
            //     .ReturnsAsync((int cityId) => new Sunset() );
            // mockSunsetRepository.Setup(repo => repo.GetByCityAndDate(It.IsAny<int>(), It.IsAny<DateTime>()))
            //     .ReturnsAsync((int cityId, DateTime date) => new Sunset() );
            // mockSunsetRepository.Setup(repo => repo.Add(It.IsAny<Sunset>()))
            //     .Returns(Task.CompletedTask);
            // mockSunsetRepository.Setup(repo => repo.Update(It.IsAny<Sunset>()))
            //     .Returns(Task.CompletedTask);
            // mockSunsetRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
            //     .Returns(Task.CompletedTask);
            //
            // // SUNRISE
            // mockSunriseRepository.Setup(repo => repo.GetByCity(It.IsAny<int>()))
            //     .ReturnsAsync((int cityId) => new Sunrise() );
            // mockSunriseRepository.Setup(repo => repo.GetByCityAndDate(It.IsAny<int>(), It.IsAny<DateTime>()))
            //     .ReturnsAsync((int cityId, DateTime date) => new Sunrise() );
            // mockSunriseRepository.Setup(repo => repo.Add(It.IsAny<Sunrise>()))
            //     .Returns(Task.CompletedTask);
            // mockSunriseRepository.Setup(repo => repo.Update(It.IsAny<Sunrise>()))
            //     .Returns(Task.CompletedTask);
            // mockSunriseRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
            //     .Returns(Task.CompletedTask);
            //
            // // Add mock repository registrations
            // services.AddSingleton(mockCityRepository.Object);
            // services.AddSingleton(mockSunsetRepository.Object);
            // services.AddSingleton(mockSunriseRepository.Object);

            // services.AddScoped<ICityDataProvider, TestCityDataProvider>();
            // services.AddScoped<IJsonProcessor, TestJsonProcessor>();
            // services.AddScoped<ISunDataProvider, TestSunDataProvider>();
            // services.AddScoped<IAuthService, TestAuthServices>();
        });
    }

    async Task SeedTestData(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<SolarWatchContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.Cities.Add(new City
            { Country = "HU", Lat = 47.4979937, Lon = 19.0403594, Name = "Budapest", State = "", Id = 1 });
        context.Sunrises.Add(new Sunrise
            { Id = 1, Date = new DateTime(2024, 04, 06), Time = "4:09:54 AM", CityId = 1 });
        context.Sunsets.Add(new Sunset
            { Id = 1, Date = new DateTime(2024, 04, 06), Time = "5:22:19 PM", CityId = 1 });

        await context.SaveChangesAsync();
    }
}