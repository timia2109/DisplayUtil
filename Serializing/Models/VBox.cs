using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Serializing.Models;

[XmlType(nameof(VBox))]
public class VBox : ICollectionXmlModel
{
    [XmlAttribute]
    public int Gap = 0;

    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return FillWithChildren(new VBoxElement(Gap), iconDrawer, fontProvider, defaults.MergeWith(Defaults));
    }
}