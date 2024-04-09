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
        context.Cities.Add(new City
            { Country = "HU2", Lat = 47.4979937, Lon = 19.0403594, Name = "Budapest2", State = "", Id = 2 });
        context.Sunrises.Add(new Sunrise
            { Id = 1, Date = new DateTime(2024, 04, 06), Time = "4:09:54 AM", CityId = 1 });
        context.Sunsets.Add(new Sunset
            { Id = 1, Date = new DateTime(2024, 04, 06), Time = "5:22:19 PM", CityId = 1 });
        context.Sunrises.Add(new Sunrise
            { Id = 2, Date = new DateTime(2024, 04, 06), Time = "4:09:54 AM", CityId = 1 });
        context.Sunsets.Add(new Sunset
            { Id = 2, Date = new DateTime(2024, 04, 06), Time = "5:22:19 PM", CityId = 1 });

        await context.SaveChangesAsync();
    }
}