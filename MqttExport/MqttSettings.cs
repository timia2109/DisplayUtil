namespace DisplayUtil.MqttExport;

public record MqttSettings
{
    public string? Uri { get; init; }
    public string? User { get; init; }
    public string? Password { get; init; }
    public string? Topic { get; init; }
    public string? ScreenDetectTemplate { get; init; }
}