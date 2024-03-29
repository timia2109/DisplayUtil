using SkiaSharp;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to render the Bitmap to the two color stream
/// </summary>
public static class BinaryImageStreamCreator
{

    public static byte[] GetImageStream(SKBitmap bitmap)
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

        return pixelWriter.ToArray();
    }

    private class PixelWriter
    {
        public void WritePixel(SKColor color)
        {
            var luma = 0.2126 * color.Red
                + 0.7152 * color.Green
                + 0.0722 * color.Blue;

            WriteBit(luma < 40);
        }

        private readonly MemoryStream _stream;
        private byte _currentByte;
        private int _bitsFilled;

        public PixelWriter()
        {
            _stream = new MemoryStream();
            _currentByte = 0;
            _bitsFilled = 0;
        }

        public void WriteBit(bool bit)
        {
            // Shift the current byte to the left by 1 and add the new bit on the end.
            _currentByte = (byte)((_currentByte << 1) | (bit ? 1 : 0));
            _bitsFilled++;

            // If the current byte is full (contains 8 bits), write it to the stream.
            if (_bitsFilled == 8)
            {
                _stream.WriteByte(_currentByte);
                // Reset for the next byte.
                _currentByte = 0;
                _bitsFilled = 0;
            }
        }

        public byte[] ToArray()
        {
            // If there are any bits that haven't been written yet because they didn't make up a full byte,
            // write them now. This will effectively pad the last byte with zeroes if it's not full.
            if (_bitsFilled > 0)
            {
                _currentByte = (byte)(_currentByte << (8 - _bitsFilled));
                _stream.WriteByte(_currentByte);
                _currentByte = 0;
                _bitsFilled = 0;
            }

            return _stream.ToArray();
        }
    }

}