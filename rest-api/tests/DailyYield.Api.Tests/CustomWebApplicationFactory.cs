using DailyYield.Adapter.Database;
using DailyYield.Integration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace DailyYield.Api.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // Configure authentication options for testing
            services.Configure<AuthenticationOptions>(options =>
            {
                options.SecretKey = "test-secret-key-for-jwt-signing-12345678901234567890";
                options.Issuer = "test-issuer";
                options.Audience = "test-audience";
                options.ExpirationHours = 1;
            });

            // Override JWT bearer configuration for testing
            services.PostConfigure<Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions>(
                Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    var key = System.Text.Encoding.ASCII.GetBytes("test-secret-key-for-jwt-signing-12345678901234567890");
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "test-issuer",
                        ValidAudience = "test-audience",
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)
                    };
                });
        });
    }
}