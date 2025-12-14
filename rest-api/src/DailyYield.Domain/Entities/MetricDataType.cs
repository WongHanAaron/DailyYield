namespace DailyYield.Domain.Entities;

/// <summary>
/// Represents the data type of a metric
/// </summary>
public enum MetricDataType
{
    /// <summary>
    /// Numeric metric with decimal values
    /// </summary>
    Numeric,

    /// <summary>
    /// Boolean metric with true/false values
    /// </summary>
    Boolean,

    /// <summary>
    /// Categorical metric with predefined category values
    /// </summary>
    Categorical
}