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
    /// <param name="defaults">Defaults for the children</param>
    /// <returns>Element</returns>
    public abstract Element AsElement(DefaultDefinition defaults);

    /// <summary>
    /// Fills the <see cref="ElementCollection"/> with the children of this Model
    /// </summary>
    /// <param name="collection">ELement Collection</param>
    /// <returns>The ElementCollection</returns>
    protected virtual ElementCollection FillWithChildren(
        ElementCollection collection, DefaultDefinition defaults
    )
    {
        collection.Append(
            Children.Select(e => e.AsElement(defaults))
        );
        return collection;
    }
}