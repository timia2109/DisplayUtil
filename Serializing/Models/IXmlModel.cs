using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

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
    public abstract Element AsElement(FaIconDrawer iconDrawer,
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
        FaIconDrawer iconDrawer, FontProvider fontProvider, DefaultDefinition defaults
    )
    {
        foreach (var child in Children)
        {
            collection.Append(child.AsElement(iconDrawer, fontProvider, defaults));
        }
        return collection;
    }
}