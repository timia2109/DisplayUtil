using System.Diagnostics;
using DisplayUtil.Scenes;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to provide the images in a form for the ESP.
/// Scoped
/// </summary>
public partial class EspImageProvider(
    ILogger<MqttExporter> logger,
    ScreenRepository screenRepository
)
{
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Gets the image as plain true / false Bytes
    /// </summary>
    /// <param name="providerId">Id of the provider</param>
    /// <returns>Byte Array</returns>
    public async Task<byte[]> GetAsPlainBytesAsync(string providerId)
    {
        var stopwatch = new Stopwatch();
        LogRender(providerId);
        stopwatch.Start();

        //Render
        using var image = await screenRepository.GetImageAsync(providerId);
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        LogRenderTime(elapsed);

        var binaryData = BinaryImageStreamCreator.GetImageStream(image);
        LogPlainBytes(binaryData.Length);
        return binaryData;
    }

    /// <summary>
    /// Gets the Image as RunLength Compressed
    /// </summary>
    /// <param name="providerId">Id of the provider</param>
    /// <returns>Compressed Data</returns>
    public async Task<byte[]> GetAsRunLengthAsync(string providerId)
    {
        var plainBytes = await GetAsPlainBytesAsync(providerId);

        var runLengthEncoder = new RunLengthCompressor();
        var compressedData = runLengthEncoder.WriteStream(plainBytes);

        var compressedPercent = compressedData.Length / (float)plainBytes.Length;
        LogCompressedBytes(
            compressedData.Length,
            Math.Round((1 - compressedPercent) * 100, 2)
        );

        return compressedData;
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