using System.Text.Json.Serialization;

namespace DisplayUtil.Awtrix;

/// <summary>
///     Moodlight configuration for setting the entire matrix to a custom color or temperature.
/// </summary>
public record AwtrixMoodlight
{
    /// <summary>
    ///     Brightness level (0-255).
    /// </summary>
    [JsonPropertyName("brightness")]
    public int? Brightness { get; init; }

    /// <summary>
    ///     Color temperature in Kelvin (e.g., 2300).
    /// </summary>
    [JsonPropertyName("kelvin")]
    public int? Kelvin { get; init; }

    /// <summary>
    ///     RGB color for the moodlight. Can be hex string or RGB array.
    /// </summary>
    [JsonPropertyName("color")]
    public Color? Color { get; init; }
}
