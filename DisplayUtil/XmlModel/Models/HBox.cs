using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public class HBox : ICollectionXmlModel
{
    [XmlAttribute] public int Gap;

    public override Element AsElement(IconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return FillWithChildren(new HBoxElement(Gap), iconDrawer, fontProvider,
            defaults.MergeWith(Defaults));
    }
}