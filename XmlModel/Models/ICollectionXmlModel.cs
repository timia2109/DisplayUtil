using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public record XmlSiteSize
{
    [XmlAttribute]
    public int Top = 0;

    [XmlAttribute]
    public int Left = 0;

    [XmlAttribute]
    public int Right = 0;

    [XmlAttribute]
    public int Bottom = 0;

    [XmlAttribute]
    public int All = 0;

    [XmlAttribute]
    public int X = 0;

    [XmlAttribute]
    public int Y = 0;

    public SiteSize AsSiteSize()
    {
        if (All != default)
            return new(All);

        if (X != default || Y != default)
            return new(X, Y);

        return new(Top, Right, Bottom, Left);
    }
}

public abstract class ICollectionXmlModel : IXmlModel
{
    public XmlSiteSize Border = new();
    public XmlSiteSize Margin = new();
    public XmlSiteSize Padding = new();

    protected override ElementCollection FillWithChildren(
        ElementCollection collection,
        FaIconDrawer iconDrawer,
        FontProvider fontProvider,
        DefaultDefinition defaults)
    {
        var collectionElement = base.FillWithChildren(
            collection, iconDrawer, fontProvider, defaults);

        collectionElement.Margin = Margin.AsSiteSize();
        collectionElement.Border = Border.AsSiteSize();
        collectionElement.Padding = Padding.AsSiteSize();

        return collectionElement;
    }
}