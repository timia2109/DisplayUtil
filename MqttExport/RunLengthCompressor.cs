using System.IO.Pipelines;

namespace DisplayUtil.MqttExport;



/// <summary>
/// Compresses the stream
/// </summary>
public class RunLengthCompressor
{
    private const ushort MaxSequence = 0b_0011_1111_1111_1111;
    private const ushort MinCountSequence = 14;
    private const ushort TrueCounter = 0b_1000_0000_0000_0000;
    private const ushort FalseCounter = 0b_0100_0000_0000_0000;
    private const ushort MaxBufferLength = MinCountSequence;

    private ushort _buffer = 0;
    private int _bufferBitsFilled = 0;

    private ushort _sequenceCount = 0;
    private bool? _sequenceType;
    private Stream Stream { get; } = new MemoryStream();


    public void WriteStream(Stream stream)
    {
        int readByte = stream.ReadByte();

        do
        {
            HandleByte((byte)readByte);
            readByte = stream.ReadByte();
        } while (readByte != -1);
        Flush();
    }

    private void HandleByte(byte b)
    {
        for (var pos = 7; pos >= 0; pos--)
        {
            var mask = (byte)(1 << pos);
            var isTrue = (b & mask) != 0;

            if (_sequenceType is null)
            {
                // Empty. Start a new sequence
                _sequenceCount = 1;
                _sequenceType = isTrue;
            }
            else if (_sequenceType == isTrue
                && _sequenceCount < MaxSequence)
            {
                // Continue sequence 
                _sequenceCount++;
            }
            else
            {
                // End of prev sequence. Start a new one
                Flush();
                _sequenceType = isTrue;
                _sequenceCount = 1;
            }
        }
    }

    private void Flush()
    {
        if (_sequenceCount == 0) return;

        if (_sequenceType is null)
            throw new InvalidDataException("Unable to handle unknown data");

        if (_sequenceCount > MinCountSequence
            && _bufferBitsFilled == 0)
        {
            // Handle Count Sequence
            var sign = _sequenceType switch
            {
                true => TrueCounter,
                false => FalseCounter
            };
            var data = (ushort)(sign | _sequenceCount);
            WriteShort(data);

            // Reset Data
            _sequenceCount = 0;
            _sequenceType = null;
        }
        else
        {
            for (; _sequenceCount > 0; _sequenceCount--)
            {
                if (AddBitToBuffer(_sequenceType.Value))
                {
                    Flush();
                    return;
                }
            }
        }
    }

    private bool AddBitToBuffer(bool bit)
    {
        _buffer = (ushort)((_buffer << 1) | (bit ? 1 : 0));
        _bufferBitsFilled++;

        if (_bufferBitsFilled == MaxBufferLength)
        {
            WriteShort(_buffer);
            _buffer = 0;
            _bufferBitsFilled = 0;
            return true;
        }

        return false;
    }

    private void WriteShort(ushort data)
    {
        Stream.WriteByte((byte)(data >> 8));
        Stream.WriteByte((byte)data);
    }
}