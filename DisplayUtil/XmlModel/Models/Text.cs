using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
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

    public override Element AsElement(DefaultDefinition defaults)
    {
        return new TextElement
        {
            Content = Content,
            Font = Font ?? defaults.Font!,
            Size = Size == 0 ? defaults.TextSize : Size
        };
    }
}