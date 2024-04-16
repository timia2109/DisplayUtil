using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.XmlModel.Models;

public class HBox : ICollectionXmlModel
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