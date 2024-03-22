
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

public class Border : IXmlModel
{
    [XmlElement]
    public Padding Padding;

    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return new BorderElement(
            Padding,
            Children.First().AsElement(iconDrawer, fontProvider,
                defaults.MergeWith(Defaults)
            )
        );
    }
}