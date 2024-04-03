using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

public class Screen : IXmlModel
{
    /// <summary>
    /// Width of this screen
    /// </summary>
    [XmlAttribute]
    public int Width;

    /// <summary>
    /// Height of this screen
    /// </summary>
    [XmlAttribute]
    public int Height;

    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return Children.First().AsElement(
            iconDrawer, fontProvider, defaults.MergeWith(Defaults)
        );
    }
}