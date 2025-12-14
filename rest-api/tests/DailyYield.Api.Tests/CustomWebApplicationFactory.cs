using DailyYield.Adapter.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DailyYield.Api.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // Remove database provider services that conflict with in-memory
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType.FullName?.Contains("Npgsql") == true ||
                d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true ||
                d.ServiceType == typeof(DailyYieldDbContext)).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Remove existing authentication configuration
            var authDescriptors = services.Where(d =>
                d.ServiceType.FullName?.Contains("Authentication") == true ||
                d.ServiceType.FullName?.Contains("JwtBearer") == true).ToList();

            foreach (var descriptor in authDescriptors)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database
            services.AddDbContext<DailyYieldDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Configure authentication options for testing
            services.Configure<DailyYield.Adapter.Database.AuthenticationOptions>(options =>
            {
                options.SecretKey = "test-secret-key-for-jwt-signing-12345678901234567890";
                options.Issuer = "test-issuer";
                options.Audience = "test-audience";
                options.ExpirationHours = 1;
            });

            // Reconfigure JWT authentication for testing
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var key = Encoding.ASCII.GetBytes("test-secret-key-for-jwt-signing-12345678901234567890");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "test-issuer",
                    ValidAudience = "test-audience",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        });
    }
}