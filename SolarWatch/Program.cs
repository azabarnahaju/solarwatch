using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SolarWatch;
using SolarWatch.Data;
using SolarWatch.Model.Enums;
using SolarWatch.Services;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SunData;

namespace SolarWatch
{
    class Program
    {
        public delegate ISolarMovementRepository ServiceResolver(string key);
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var issuerSigningKey = builder.Configuration["JwtSettings:IssuerSigningKey"];
            
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
                        ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(issuerSigningKey)
                        ),
                    };
                });
            
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<ICityDataProvider, CityDataProvider>();
            builder.Services.AddSingleton<ISunDataProvider, SunDataProvider>();
            builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
            builder.Services.AddSingleton<ICityRepository, CityRepository>();
            builder.Services.AddTransient<SunsetRepository>();
            builder.Services.AddTransient<SunriseRepository>();
            builder.Services.AddTransient<ServiceResolver>(ServiceProvider => key =>
            {
                switch (key)
                {
                    case "Sunrise":
                        return ServiceProvider.GetService<SunriseRepository>();
                    case "Sunset":
                        return ServiceProvider.GetService<SunsetRepository>();
                    default:
                        throw new KeyNotFoundException();
                }
            });
            builder.Services.AddDbContext<UsersContext>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}