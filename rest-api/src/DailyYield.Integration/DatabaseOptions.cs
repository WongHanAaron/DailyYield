namespace DailyYield.Integration;

/// <summary>
/// Options for database configuration
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// Gets or sets the database provider type
    /// </summary>
    public DatabaseProvider Provider { get; set; } = DatabaseProvider.PostgreSQL;

    /// <summary>
    /// Gets or sets the connection string for PostgreSQL
    /// </summary>
    public string? ConnectionString { get; set; }
}

/// <summary>
/// Enum for database provider types
/// </summary>
public enum DatabaseProvider
{
    /// <summary>
    /// Use PostgreSQL database
    /// </summary>
    PostgreSQL,

    /// <summary>
    /// Use in-memory database
    /// </summary>
    InMemory
}