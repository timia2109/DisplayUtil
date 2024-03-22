using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Serializing.Models;

public class HBox : IXmlModel
{
    [XmlAttribute]
    public int Gap = 0;

    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return FillWithChildren(new HBoxElement(Gap), iconDrawer, fontProvider,
            defaults.MergeWith(Defaults));
    }
}