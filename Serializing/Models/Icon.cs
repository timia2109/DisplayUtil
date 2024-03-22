
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

public class Icon : IXmlModel
{
    [Required]
    [XmlAttribute]
    public string IconName = null!;

    [XmlAttribute]
    public int? Height = null;

    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return new IconElement(IconName, Height ?? defaults.IconHeight!.Value, iconDrawer);
    }
}