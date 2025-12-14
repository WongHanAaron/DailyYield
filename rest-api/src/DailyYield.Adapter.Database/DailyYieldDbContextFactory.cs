using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DailyYield.Adapter.Database;

/// <summary>
/// Factory for creating DailyYieldDbContext instances at design time (for migrations)
/// </summary>
public class DailyYieldDbContextFactory : IDesignTimeDbContextFactory<DailyYieldDbContext>
{
    public DailyYieldDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        // Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Create options
        var optionsBuilder = new DbContextOptionsBuilder<DailyYieldDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DailyYieldDbContext(optionsBuilder.Options);
    }
}