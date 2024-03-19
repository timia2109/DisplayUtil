using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing.Models;

public abstract class IXmlModel
{
    public IXmlModel[] Children = null!;

    /// <summary>
    /// Returns the corrosponding Element
    /// </summary>
    /// <param name="iconDrawer">Icon Drawer</param>
    /// <param name="fontProvider">Font Provider</param>
    /// <returns>Element</returns>
    public abstract Element AsElement(FaIconDrawer iconDrawer, FontProvider fontProvider);

    /// <summary>
    /// Fills the <see cref="ElementCollection"/> with the children of this Model
    /// </summary>
    /// <param name="collection">ELement Collection</param>
    /// <param name="iconDrawer">Icon Drawer</param>
    /// <param name="fontProvider">FontProvider</param>
    /// <returns>The ElementCollection</returns>
    protected ElementCollection FillWithChildren(
        ElementCollection collection,
        FaIconDrawer iconDrawer, FontProvider fontProvider
    )
    {
        foreach (var child in Children)
        {
            collection.Append(child.AsElement(iconDrawer, fontProvider));
        }
        return collection;
    }
}