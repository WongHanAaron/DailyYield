namespace DailyYield.Adapter.Database;

/// <summary>
/// Configuration options for authentication services
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// The secret key used for JWT token signing
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// The JWT token issuer
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The JWT token audience
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time in hours
    /// </summary>
    public int ExpirationHours { get; set; } = 24;
}