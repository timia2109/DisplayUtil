using System.Diagnostics;
using DisplayUtil.EspUtilities;
using DisplayUtil.Scenes;
using Microsoft.Extensions.Options;
using MQTTnet.Client;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to export the image to Mqtt.
/// Scoped
/// </summary>
public class MqttExporter(
    ExportingMqttClient exportingMqttClient,
    IOptions<MqttSettings> options
)
{
    public Task ExportUriToMqtt(string providerId)
    {
        var settings = options.Value;

        var query = providerId.IndexOf('?');
        var providerPath = query == -1
            ? providerId
            : providerId[0..(query - 1)];

        var uriBuilder = new UriBuilder
        {
            Port = 80,
            Scheme = "http",
            Host = settings.ServerHostName,
            Path = EspUtilitiesInitExtension.CompressedImageRoute
        };

        uriBuilder.Path = EspUtilitiesInitExtension.CompressedImageRoute
            .Replace("{providerId}", providerPath);

        if (query != -1)
            uriBuilder.Query = providerId[query..];

        return SubmitAsync(uriBuilder.Uri);
    }

    public virtual async Task SubmitAsync(Uri uri)
    {
        await exportingMqttClient.SendAsync(uri.ToString());
    }
}

internal partial class CachedMqttExporter(
    ExportingMqttClient exportingMqttClient,
    IOptions<MqttSettings> options,
    ILogger<CachedMqttExporter> logger)
    : MqttExporter(exportingMqttClient, options)
{
    private readonly ILogger _logger = logger;

    private Uri? _lastSubmission;

    public override Task SubmitAsync(Uri uri)
    {
        if (_lastSubmission == uri)
        {
            LogSkip(_lastSubmission);
            return Task.CompletedTask;
        }

        LogSubmitting(uri);
        _lastSubmission = uri;
        return base.SubmitAsync(uri);
    }

    [LoggerMessage(LogLevel.Debug, "Skipping resubmission of {uri}")]
    private partial void LogSkip(Uri uri);

    [LoggerMessage(LogLevel.Debug, "Submitting Uri {uri}")]
    private partial void LogSubmitting(Uri uri);
}