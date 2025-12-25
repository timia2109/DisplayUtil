using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

[XmlType(nameof(VBox))]
public class VBox : ICollectionXmlModel
{
    [XmlAttribute] public int Gap;

    public override Element AsElement(IconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return FillWithChildren(new VBoxElement(Gap), iconDrawer, fontProvider, defaults.MergeWith(Defaults));
    }
}