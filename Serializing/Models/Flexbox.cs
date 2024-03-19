using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

public class Flexbox : IXmlModel
{

    [XmlAttribute]
    public FlexDirection Direction = FlexDirection.Horizontal;

    [XmlAttribute]
    public JustifyContent JustifyContent = JustifyContent.Between;

    [XmlAttribute]
    public AlignItems AlignItems = AlignItems.Start;

    [XmlAttribute]
    public int Gap = 0;

    public override Element AsElement(FaIconDrawer iconDrawer, FontProvider fontProvider)
    {
        return FillWithChildren(
            new FlexboxElement(Gap, Direction, JustifyContent, AlignItems),
            iconDrawer, fontProvider
        );
    }
}