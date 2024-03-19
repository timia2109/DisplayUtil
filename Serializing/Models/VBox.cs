using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Serializing.Models;

[XmlType(nameof(VBox))]
public class VBox : IXmlModel
{
    [XmlAttribute]
    public int Gap = 0;

    public override Element AsElement(FaIconDrawer iconDrawer, FontProvider fontProvider)
    {
        return FillWithChildren(new VBoxElement(Gap), iconDrawer, fontProvider);
    }
}