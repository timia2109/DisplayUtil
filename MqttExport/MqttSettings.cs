namespace DisplayUtil.MqttExport;

public record MqttSettings
{
    public string Uri { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}