using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Serializing.Models;

[XmlType(nameof(Text))]
public class Text : IXmlModel
{
    [XmlAttribute]
    [Required]
    public string Content = null!;

    [XmlAttribute]
    public string? Font;

    [XmlAttribute]
    public int Size = 20;

    public override Element AsElement(FaIconDrawer iconDrawer, FontProvider fontProvider)
    {
        var font = fontProvider.GetFont(Font ?? "Roboto-Medium");

        var paint = new SKPaint
        {
            IsAntialias = true,
            TextSize = Size,
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            Typeface = font
        };

        return new TextElement(Content, paint);
    }
}