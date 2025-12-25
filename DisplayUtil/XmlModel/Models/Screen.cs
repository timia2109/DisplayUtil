using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public class Screen : IXmlModel
{
    /// <summary>
    ///     Height of this screen
    /// </summary>
    [XmlAttribute] public int Height;

    /// <summary>
    ///     Width of this screen
    /// </summary>
    [XmlAttribute] public int Width;

    public override Element AsElement(IconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        return Children.First().AsElement(
            iconDrawer, fontProvider, defaults.MergeWith(Defaults)
        );
    }
}