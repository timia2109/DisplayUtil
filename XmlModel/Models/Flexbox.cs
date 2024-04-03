using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public class Flexbox : ICollectionXmlModel
{

    [XmlAttribute]
    public FlexDirection Direction = FlexDirection.Horizontal;

    [XmlAttribute]
    public JustifyContent JustifyContent = JustifyContent.Between;

    [XmlAttribute]
    public AlignItems AlignItems = AlignItems.Start;

    [XmlAttribute]
    public int Gap = 0;

    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return FillWithChildren(
            new FlexboxElement(Gap, Direction, JustifyContent, AlignItems),
            iconDrawer, fontProvider,
            defaults.MergeWith(Defaults)
        );
    }
}