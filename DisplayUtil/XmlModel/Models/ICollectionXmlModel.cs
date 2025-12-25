using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;

namespace DisplayUtil.XmlModel.Models;

public record XmlSiteSize
{
    [XmlAttribute] public int All;

    [XmlAttribute] public int Bottom;

    [XmlAttribute] public int Left;

    [XmlAttribute] public int Right;

    [XmlAttribute] public int Top;

    [XmlAttribute] public int X;

    [XmlAttribute] public int Y;

    public SiteSize AsSiteSize()
    {
        if (All != default)
            return new SiteSize(All);

        if (X != default || Y != default)
            return new SiteSize(X, Y);

        return new SiteSize(Top, Right, Bottom, Left);
    }
}

public abstract class ICollectionXmlModel : IXmlModel
{
    public XmlSiteSize Border = new();
    public XmlSiteSize Margin = new();
    public XmlSiteSize Padding = new();

    protected override ElementCollection FillWithChildren(
        ElementCollection collection,
        IconDrawer iconDrawer,
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