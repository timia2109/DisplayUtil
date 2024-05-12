using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

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

    public override Element AsElement(DefaultDefinition defaults)
    {
        return Children.First().AsElement(
            defaults.MergeWith(Defaults)
        );
    }
}