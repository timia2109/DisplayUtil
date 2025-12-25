namespace DisplayUtil.Awtrix;

/// <summary>
///     Settings for Awtrix MQTT connection.
/// </summary>
public record AwtrixConnectionOptions
{
    /// <summary>
    ///     Affected topic on the Awtrix MQTT broker.
    /// </summary>
    public required string MqttTopic { get; init; }

    /// <summary>
    ///     Host of the MQTT broker.
    /// </summary>
    public string? Host { get; init; }

    /// <summary>
    ///     Port of the MQTT broker.
    /// </summary>
    public int Port { get; init; } = 1883;

    /// <summary>
    ///     Username of the MQTT broker.
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    ///     Password of the MQTT broker.
    /// </summary>
    public string? Password { get; init; }

    /// <summary>
    /// Client Id of the MQTT broker.
    /// </summary>
    public string? ClientId { get; init; }
}