using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch;
using SolarWatch.Data;
using SolarWatch.Model.Enums;
using SolarWatch.Services;
using SolarWatch.Services.Authentication;
using SolarWatch.Services.CityData;
using SolarWatch.Services.JsonProcessing;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SunData;

namespace SolarWatch
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var userSecrets = new Dictionary<string, string>
            {
                { "validIssuer", builder.Configuration["JwtSettings:ValidIssuer"] },
                { "validAudience", builder.Configuration["JwtSettings:validAudience"] },
                { "issuerSigningKey", builder.Configuration["JwtSettings:IssuerSigningKey"] },
                { "dbConnectionString", builder.Configuration["Database:ConnectionString"] },
                { "adminEmail", builder.Configuration["AdminInfo:adminEmail"] }, 
                { "adminPassword", builder.Configuration["AdminInfo:adminPassword"] }
            };

            if (environment == "test")
            {
                userSecrets["issuerSigningKey"] = "This_is_a_super_secure_key_and_you_know_it";
            }
            
            foreach (var secret in userSecrets)
            {
                if (secret.Value is null)
                {
                    throw new Exception($"{secret.Key} is missing.");
                }
            }
            
            AddServices();
            ConfigureSwagger();
            AddDbContext();
            AddAuthentication();
            AddIdentity();
            
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();

            if (authenticationSeeder is not null)
            {
                Console.WriteLine("AuthenticationSeeder retrieved successfully.");
                authenticationSeeder.AddRoles();
                authenticationSeeder.AddAdmin();
            }
            else
            {
                Console.WriteLine("Error: AuthenticationSeeder could not be found.");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173"));
            
            app.UseHttpsRedirection();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            
            // method implementations

            void AddServices()
            {
                builder.Services.AddCors();
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
            
                builder.Services.AddTransient<ICityDataProvider, CityDataProvider>();
                builder.Services.AddTransient<ISunDataProvider, SunDataProvider>();
                builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
                builder.Services.AddTransient<ICityRepository, CityRepository>();
                builder.Services.AddTransient<ISunsetRepository, SunsetRepository>();
                builder.Services.AddTransient<ISunriseRepository, SunriseRepository>();
                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<ITokenService>(provider =>
                    new TokenService(userSecrets["validIssuer"], userSecrets["validAudience"], userSecrets["issuerSigningKey"]));
                builder.Services.AddScoped<AuthenticationSeeder>(provider =>
                {
                    var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
            
                    // Fetch adminInfo from configuration or any other source
                    var adminInfo = new Dictionary<string, string>
                    {
                        {"adminEmail", userSecrets["adminEmail"]},
                        {"adminPassword", userSecrets["adminPassword"]}
                    };

                    return new AuthenticationSeeder(roleManager, userManager, adminInfo);
                });
            }

            void ConfigureSwagger()
            {
                builder.Services.AddSwaggerGen(option =>
                {
                    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                });
            }

            void AddDbContext()
            {
                builder.Services.AddDbContext<SolarWatchContext>(options => options.UseSqlServer(userSecrets["dbConnectionString"]));
                builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(userSecrets["dbConnectionString"]));
            }
            
            void AddAuthentication()
            {
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
                            ValidIssuer = userSecrets["validIssuer"],
                            ValidAudience = userSecrets["validAudience"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(userSecrets["issuerSigningKey"])
                            ),
                        };
                    });
            }

            void AddIdentity()
            {
                builder.Services
                    .AddIdentityCore<IdentityUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = true;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = true;
                        options.Password.RequireLowercase = true;
                    })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<UsersContext>();
            }
        }
    }
}