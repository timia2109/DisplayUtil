using System.Text.Json;
using System.Text.Json.Serialization;

namespace DisplayUtil.Awtrix;

[JsonConverter(typeof(ColorJsonConverter))]
public record Color 
{
    public byte Red { get; init; }
    public byte Green { get; init; }
    public byte Blue { get; init; }

    public static Color FromString(string color)
    {
        if (color.Length is 6 or 8)
        {
            var red = Convert.ToByte(color.Substring(1, 2), 16);
            var green = Convert.ToByte(color.Substring(3, 2), 16);
            var blue = Convert.ToByte(color.Substring(5, 2), 16);
            return new Color { Red = red, Green = green, Blue = blue };
        }

        throw new ArgumentException("Invalid color format", nameof(color));
    }

    public override string ToString()
    {
        return $"{Red:X2}{Green:X2}{Blue:X2}";
    }
}

public sealed class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var colorString = reader.GetString();
        if (colorString is null)
        {
            throw new JsonException("Color string is null");
        }
        return Color.FromString(colorString);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

