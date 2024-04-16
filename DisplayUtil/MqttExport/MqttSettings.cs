namespace DisplayUtil.MqttExport;

/// <summary>
/// Settings for the MQTT Handling
/// </summary>
public record MqttSettings
{
    /// <summary>
    /// URI of MQTT Server
    /// </summary>
    public string? Uri { get; init; }

    /// <summary>
    /// User 
    /// </summary>
    public string? User { get; init; }

    /// <summary>
    /// Password 
    /// </summary>
    public string? Password { get; init; }

    /// <summary>
    /// MQTT Topic
    /// </summary>
    public string? Topic { get; init; }

    /// <summary>
    /// Template used, to detect the MQTT Template
    /// </summary>
    public string? ScreenDetectTemplate { get; init; }

    /// <summary>
    /// Interval for rerendering the Template
    /// </summary>
    public TimeSpan? RefreshInterval { get; init; }

    /// <summary>
    /// Should the message only be updated, when the value changes?
    /// </summary>
    public bool IncrementalUpdate { get; init; }

    /// <summary>
    /// Hostname of the current server
    /// </summary>
    public string? ServerHostName { get; init; }
}