
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public class Icon : IXmlModel
{
    [Required]
    [XmlAttribute]
    public string IconName = null!;

    [XmlAttribute]
    public int Height;

    public override Element AsElement(IconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return new IconElement(IconName,
            Height == 0 ? defaults.IconHeight : Height,
            iconDrawer);
    }
}