
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
    public int Height = 20;

    public override Element AsElement(FaIconDrawer iconDrawer, FontProvider fontProvider)
    {
        return new IconElement(IconName, Height, iconDrawer);
    }
}