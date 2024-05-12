using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.XmlModel.Models;

[XmlType(nameof(VBox))]
public class VBox : ICollectionXmlModel
{
    [XmlAttribute]
    public int Gap = 0;

    public override Element AsElement(DefaultDefinition defaults)
    {
        return FillWithChildren(
            new VBoxElement
            {
                Gap = Gap,
            },
            defaults.MergeWith(Defaults)
        );
    }
}