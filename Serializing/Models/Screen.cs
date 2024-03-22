using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

public class Screen : IXmlModel
{
    public override Element AsElement(FaIconDrawer iconDrawer,
        FontProvider fontProvider, DefaultDefinition defaults)
    {
        var useDefaults = Defaults ?? defaults;
        return Children.First().AsElement(
            iconDrawer, fontProvider, useDefaults
        );
    }
}