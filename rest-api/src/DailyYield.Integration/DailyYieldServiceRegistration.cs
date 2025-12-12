using DailyYield.Adapter.Database;
using DailyYield.Application;
using DailyYield.Domain.Ports;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DailyYield.Integration;

/// <summary>
/// Extension methods for registering all DailyYield services
/// </summary>
public static class DailyYieldServiceRegistration
{
    /// <summary>
    /// Registers all DailyYield services with the dependency injection container
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection with all services registered</returns>
    public static IServiceCollection AddDailyYieldServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register all service categories
        services.AddApplicationLayerServices();
        services.AddInfrastructureServices(configuration);

        return services;
    }

    /// <summary>
    /// Registers application layer services (CQRS, AutoMapper, etc.)
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection with application services registered</returns>
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        // Register application layer services
        services.AddApplicationServices();

        return services;
    }

    /// <summary>
    /// Registers infrastructure services (database, authentication, etc.)
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection with infrastructure services registered</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register all infrastructure service categories
        services.AddDatabaseServices(configuration);
        services.AddAuthenticationServices(configuration);
        services.AddCorsServices();

        return services;
    }

    /// <summary>
    /// Registers database services (DbContext, repositories, etc.)
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection with database services registered</returns>
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database context
        services.AddDbContext<DailyYieldDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

        return services;
    }

    /// <summary>
    /// Registers authentication and authorization services
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection with authentication services registered</returns>
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure authentication options from configuration
        services.Configure<AuthenticationOptions>(configuration.GetSection("Jwt"));

        // Register authentication service
        services.AddScoped<IAuthService, AuthService>();

        // JWT Authentication
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "default-secret-key");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"] ?? "DailyYield",
                ValidAudience = jwtSettings["Audience"] ?? "DailyYieldUsers",
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.AddAuthorization();

        return services;
    }

    /// <summary>
    /// Registers CORS services
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection with CORS services registered</returns>
    public static IServiceCollection AddCorsServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }
}
