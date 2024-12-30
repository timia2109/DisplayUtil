using System.Diagnostics;
using DisplayUtil.Infrastructure.Providers.Image;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace DisplayUtil.Infrastructure.EspUtilities;

/// <summary>
/// Responsible to provide the images in a form for the ESP.
/// Scoped
/// </summary>
public partial class EspImageProvider(
    ILogger<EspImageProvider> logger,
    ImageResolver imageResolver
)
{
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Gets the image as plain true / false Bytes
    /// </summary>
    /// <param name="imageId">Id of the provider</param>
    /// <returns>Byte Array or null if the image was not found</returns>
    public async Task<(byte[], SKSize)?> GetAsPlainBytesAsync(string imageId)
    {
        var stopwatch = new Stopwatch();
        LogRender(imageId);
        stopwatch.Start();

        //Render
        using var image = await imageResolver.GetImageAsync(imageId);
        if (image == null) return null;

        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        LogRenderTime(elapsed);

        var binaryData = BinaryImageStreamCreator.GetImageStream(image);
        LogPlainBytes(binaryData.Length);
        return (binaryData, new SKSize(image.Width, image.Height));
    }

    /// <summary>
    /// Gets the Image as RunLength Compressed
    /// </summary>
    /// <param name="imageId">Id of the provider</param>
    /// <returns>Compressed Data</returns>
    public async Task<(byte[], SKSize)?> GetAsRunLengthAsync(string imageId)
    {
        var result = await GetAsPlainBytesAsync(imageId);
        if (result == null) return null;

        var (plainBytes, size) = result.Value;

        var runLengthEncoder = new RunLengthCompressor();
        var compressedData = runLengthEncoder.WriteStream(plainBytes);

        var compressedPercent = compressedData.Length / (float)plainBytes.Length;
        LogCompressedBytes(
            compressedData.Length,
            Math.Round((1 - compressedPercent) * 100, 2)
        );

        return (compressedData, size);
    }


    [LoggerMessage(LogLevel.Debug, "Render screen {imageId}")]
    private partial void LogRender(string imageId);

    [LoggerMessage(LogLevel.Debug, "Rendering took {time} ms")]
    private partial void LogRenderTime(long time);

    [LoggerMessage(LogLevel.Debug, "Plain binary takes {bytes} bytes")]
    private partial void LogPlainBytes(int bytes);

    [LoggerMessage(LogLevel.Debug, "Compressing takes {bytes} bytes ({percent} % less)")]
    private partial void LogCompressedBytes(int bytes, double percent);
}