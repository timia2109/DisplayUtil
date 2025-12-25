using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public class Flexbox : ICollectionXmlModel
{
    [XmlAttribute] public AlignItems AlignItems = AlignItems.Start;

    [XmlAttribute] public FlexDirection Direction = FlexDirection.Horizontal;

    [XmlAttribute] public int Gap;

    [XmlAttribute] public JustifyContent JustifyContent = JustifyContent.Between;

    public override Element AsElement(IconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return FillWithChildren(
            new FlexboxElement(Gap, Direction, JustifyContent, AlignItems),
            iconDrawer, fontProvider,
            defaults.MergeWith(Defaults)
        );
    }
}