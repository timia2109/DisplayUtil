using System.Text.Json.Serialization;

namespace DisplayUtil.Awtrix;

/// <summary>
///     Colored indicator configuration.
/// </summary>
public record AwtrixIndicator
{
    /// <summary>
    ///     Color for the indicator. Can be RGB array or hex color string.
    ///     Use black ([0,0,0]) or "0" to hide the indicator.
    /// </summary>
    [JsonPropertyName("color")]
    public Color? Color { get; init; }

    /// <summary>
    ///     Blinking interval in milliseconds. Optional.
    /// </summary>
    [JsonPropertyName("blink")]
    public int? Blink { get; init; }

    /// <summary>
    ///     Fade interval in milliseconds. Optional.
    /// </summary>
    [JsonPropertyName("fade")]
    public int? Fade { get; init; }
}
