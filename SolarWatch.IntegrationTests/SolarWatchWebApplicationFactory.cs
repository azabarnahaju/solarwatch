﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using SolarWatch.Data;
using SolarWatch.IntegrationTests.JwtAuthenticationTest;
using SolarWatch.IntegrationTests.TestServices;
using SolarWatch.Model;
using SolarWatch.Services.Authentication;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SunData;

namespace SolarWatch.IntegrationTests;

public class SolarWatchWebApplicationFactory<Program> : WebApplicationFactory<Program> where Program : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // initializing databases
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));
            var userContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UsersContext>));

            services.Remove(dbContextDescriptor);
            services.Remove(userContextDescriptor);
            
            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase("SolarWatch_Test");
            });
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseInMemoryDatabase("SolarWatch_Users_Test");
            });
            
            // adding JWT authorization
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
                    options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                }
            );
            
            // adding mock repositories
            var cityRepositoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICityRepository));
            if (cityRepositoryDescriptor != null)
                services.Remove(cityRepositoryDescriptor);
            var sunriseRepositoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISunriseRepository));
            if (sunriseRepositoryDescriptor != null)
                services.Remove(sunriseRepositoryDescriptor);
            var sunsetRepositoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISunsetRepository));
            if (sunsetRepositoryDescriptor != null)
                services.Remove(sunsetRepositoryDescriptor);

            // Create mock repository instances
            var mockCityRepository = new Mock<ICityRepository>();
            var mockSunsetRepository = new Mock<ISunsetRepository>();
            var mockSunriseRepository = new Mock<ISunriseRepository>();

            // CITY
            mockCityRepository.Setup(repo => repo.GetCity(It.IsAny<string>()))
                .ReturnsAsync((string cityName) => new City { /* mocked city object */ });
            mockCityRepository.Setup(repo => repo.Add(It.IsAny<City>()))
                .Returns(Task.CompletedTask);
            mockCityRepository.Setup(repo => repo.Update(It.IsAny<City>()))
                .Returns(Task.CompletedTask);
            mockCityRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            
            // SUNSET
            mockSunsetRepository.Setup(repo => repo.GetByCity(It.IsAny<int>()))
                .ReturnsAsync((int cityId) => new Sunset() );
            mockSunsetRepository.Setup(repo => repo.GetByCityAndDate(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync((int cityId, DateTime date) => new Sunset() );
            mockSunsetRepository.Setup(repo => repo.Add(It.IsAny<Sunset>()))
                .Returns(Task.CompletedTask);
            mockSunsetRepository.Setup(repo => repo.Update(It.IsAny<Sunset>()))
                .Returns(Task.CompletedTask);
            mockSunsetRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            
            // SUNRISE
            mockSunriseRepository.Setup(repo => repo.GetByCity(It.IsAny<int>()))
                .ReturnsAsync((int cityId) => new Sunrise() );
            mockSunriseRepository.Setup(repo => repo.GetByCityAndDate(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync((int cityId, DateTime date) => new Sunrise() );
            mockSunriseRepository.Setup(repo => repo.Add(It.IsAny<Sunrise>()))
                .Returns(Task.CompletedTask);
            mockSunriseRepository.Setup(repo => repo.Update(It.IsAny<Sunrise>()))
                .Returns(Task.CompletedTask);
            mockSunriseRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            
            // Add mock repository registrations
            services.AddSingleton(mockCityRepository.Object);
            services.AddSingleton(mockSunsetRepository.Object);
            services.AddSingleton(mockSunriseRepository.Object);
            
            services.AddScoped<ICityDataProvider, TestCityDataProvider>();
            services.AddScoped<IJsonProcessor, TestJsonProcessor>();
            services.AddScoped<ISunDataProvider, TestSunDataProvider>();
            services.AddScoped<IAuthService, TestAuthServices>();
        });
        
        builder.UseEnvironment("Development");
    }
}