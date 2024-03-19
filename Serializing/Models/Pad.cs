
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

public class Pad : IXmlModel
{
    [XmlElement]
    public Padding Padding;

    public override Element AsElement(FaIconDrawer iconDrawer, FontProvider fontProvider)
    {
        return new PaddingElement(
            Padding,
            Children.First().AsElement(iconDrawer, fontProvider)
        );
    }
}