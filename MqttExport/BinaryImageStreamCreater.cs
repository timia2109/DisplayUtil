using SkiaSharp;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to render the Bitmap to the two color stream
/// </summary>
public static class BinaryImageStreamCreator
{

    public static Stream GetImageStream(SKBitmap bitmap)
    {
        var pixelWriter = new PixelWriter();

        var width = bitmap.Width;
        var height = bitmap.Height;

        // The stream is width -> height
        for (var y = 1; y <= height; y++)
        {
            for (var x = 1; x <= width; x++)
            {
                var pixel = bitmap.GetPixel(x, y);
                pixelWriter.WritePixel(pixel);
            }
        }

        return pixelWriter.Stream;
    }

    private class PixelWriter
    {
        public Stream Stream
        {
            get
            {
                Flush();
                _stream.Position = 0;
                return _stream;
            }
        }

        private readonly Stream _stream = new MemoryStream();
        private byte _currentBytePosition = 7;
        private int _buffer = 0;

        public void WritePixel(SKColor color)
        {
            WriteFilled(color == SKColors.Black);
        }

        public void WriteFilled(bool isFilled)
        {
            if (isFilled)
                _buffer |= 1 << _currentBytePosition;

            _currentBytePosition--;
            if (_currentBytePosition < 0)
            {
                Flush();
            }
        }

        public void Flush()
        {
            if (_buffer == 0) return;

            _stream.WriteByte((byte)_buffer);
            _buffer = 0;
            _currentBytePosition = 7;
        }
    }

}