using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public abstract class IXmlModel
{
    public IXmlModel[] Children = null!;

    /// <summary>
    /// Defaults
    /// </summary>
    public DefaultDefinition? Defaults = null;

    /// <summary>
    /// Returns the corrosponding Element
    /// </summary>
    /// <param name="iconDrawer">Icon Drawer</param>
    /// <param name="fontProvider">Font Provider</param>
    /// <param name="defaults">Defaults for the children</param>
    /// <returns>Element</returns>
    public abstract Element AsElement(IconDrawer iconDrawer,
        FontProvider fontProvider,
        DefaultDefinition defaults
    );

    /// <summary>
    /// Fills the <see cref="ElementCollection"/> with the children of this Model
    /// </summary>
    /// <param name="collection">ELement Collection</param>
    /// <param name="iconDrawer">Icon Drawer</param>
    /// <param name="fontProvider">FontProvider</param>
    /// <returns>The ElementCollection</returns>
    protected virtual ElementCollection FillWithChildren(
        ElementCollection collection,
        IconDrawer iconDrawer, FontProvider fontProvider, DefaultDefinition defaults
    )
    {
        collection.Append(
            Children.Select(e => e.AsElement(iconDrawer, fontProvider, defaults))
        );
        return collection;
    }
}