using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.XmlModel.Models;

[XmlType(nameof(Text))]
public class Text : IXmlModel
{
    [XmlAttribute]
    [Required]
    public string Content = null!;

    [XmlAttribute]
    public string? Font;

    [XmlAttribute]
    public int Size;

    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        var font = fontProvider.GetFont(Font ?? defaults.Font!);

        var paint = new SKPaint
        {
            IsAntialias = true,
            TextSize = Size == 0 ? defaults.TextSize : Size,
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            Typeface = font
        };

        return new TextElement(Content, paint);
    }
}