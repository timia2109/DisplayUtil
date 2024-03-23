namespace DisplayUtil.MqttExport;

public record MqttSettings
{
    public string? Uri { get; init; } = null!;
    public string? User { get; init; } = null!;
    public string? Password { get; init; } = null!;
    public string? ScreenDetectTemplate { get; init; }
}