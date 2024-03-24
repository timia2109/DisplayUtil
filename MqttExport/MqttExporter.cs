using System.Diagnostics;
using DisplayUtil.Scenes;
using MQTTnet.Client;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to export the image to Mqtt.
/// Scoped
/// </summary>
public partial class MqttExporter(
    ILogger<MqttExporter> logger,
    ScreenRepository screenRepository,
    ExportingMqttClient exportingMqttClient
)
{
    private readonly ILogger _logger = logger;

    public async Task ExportScreenToMqtt(string providerId)
    {
        var stopwatch = new Stopwatch();
        LogRender(providerId);
        stopwatch.Start();

        //Render
        using var image = await screenRepository.GetImageAsync(providerId);
        stopwatch.Stop();
        LogRenderTime(stopwatch.ElapsedMilliseconds);

        var binaryData = BinaryImageStreamCreator.GetImageStream(image);
        LogPlainBytes(binaryData.Length);

        var runLengthEncoder = new RunLengthCompressor();
        var compressedData = runLengthEncoder.WriteStream(binaryData);

        var compressedPercent = compressedData.Length / (float)binaryData.Length;
        LogCompressedBytes(
            compressedData.Length,
            Math.Round((1 - compressedPercent) * 100, 2)
        );

        await exportingMqttClient.SendAsync(compressedData);
    }

    [LoggerMessage(LogLevel.Information, "Render screen {providerId}")]
    private partial void LogRender(string providerId);

    [LoggerMessage(LogLevel.Information, "Rendering took {time} ms")]
    private partial void LogRenderTime(long time);

    [LoggerMessage(LogLevel.Information, "Plain binary takes {bytes} bytes")]
    private partial void LogPlainBytes(int bytes);

    [LoggerMessage(LogLevel.Information, "Compressing takes {bytes} bytes ({percent} % less)")]
    private partial void LogCompressedBytes(int bytes, double percent);
}