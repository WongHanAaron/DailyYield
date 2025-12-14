using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DailyYield.Application;

/// <summary>
/// Extension methods for registering application layer services
/// </summary>
public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Registers all application layer services with the dependency injection container
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection with application services registered</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR for CQRS pattern
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
        });

        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        // Register AutoMapper
        services.AddAutoMapper(typeof(ApplicationServiceRegistration).Assembly);

        return services;
    }
}