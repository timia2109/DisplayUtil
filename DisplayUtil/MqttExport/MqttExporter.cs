namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to export the image to Mqtt.
/// Scoped
/// </summary>
public class MqttExporter(
    ExportingMqttClient exportingMqttClient
)
{
    public virtual async Task PublishUriToMqttAsync(Uri uri)
    {
        await exportingMqttClient.SendAsync(uri.ToString());
    }
}

internal partial class CachedMqttExporter(
    ExportingMqttClient exportingMqttClient,
    ILogger<CachedMqttExporter> logger)
    : MqttExporter(exportingMqttClient)
{
    private readonly ILogger _logger = logger;

    private Uri? _lastSubmission;

    public override Task PublishUriToMqttAsync(Uri uri)
    {
        if (_lastSubmission == uri)
        {
            LogSkip(_lastSubmission);
            return Task.CompletedTask;
        }

        LogSubmitting(uri);
        _lastSubmission = uri;
        return base.PublishUriToMqttAsync(uri);
    }

    [LoggerMessage(LogLevel.Debug, "Skipping resubmission of {uri}")]
    private partial void LogSkip(Uri uri);

    [LoggerMessage(LogLevel.Debug, "Submitting Uri {uri}")]
    private partial void LogSubmitting(Uri uri);
}