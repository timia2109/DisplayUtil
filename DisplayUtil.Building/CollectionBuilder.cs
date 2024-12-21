using DisplayUtil.Building.Font;
using DisplayUtil.Building.Items;
using DisplayUtil.Layouting;
using DisplayUtil.Layouting.Utils;

namespace DisplayUtil.Building;

internal class PrebuildElement(Element element) : ILayoutBuilder
{
    public Element Build() => element;
}

public abstract class CollectionBuilder<TBuilder>(
    IFontProvider fontProvider, IIconDrawer iconDrawer,
    DefaultDefinition defaultDefinition)
    : LayoutBuilder<TBuilder>(fontProvider, iconDrawer, defaultDefinition)
    where TBuilder : CollectionBuilder<TBuilder>
{
    protected List<ILayoutBuilder> children = [];

    public TBuilder WithElement(Element element)
    {
        children.Add(new PrebuildElement(element));
        return (TBuilder)this;
    }

    public TBuilder WithChildBuilder(ILayoutBuilder builder)
    {
        children.Add(builder);
        return (TBuilder)this;
    }

    public TBuilder WithText(string text,
        string? fontName = null,
        int? fontSize = null)
    {
        return WithElement(CreateText(text,
            fontName,
            fontSize));
    }

    public TBuilder WithIcon(string iconName, int? height = null)
    {
        return WithElement(CreateIcon(iconName, height));
    }

    protected TElement FillChildren<TElement>(TElement element)
        where TElement : ElementCollection
    {
        element.Children = children.Select(c => c.Build()).ToList();
        return element;
    }

    public HBoxBuilder AddHBox()
    {
        var hbox = new HBoxBuilder(fontProvider, iconDrawer, DefaultDefinition);
        children.Add(hbox);
        return hbox;
    }

    public VBoxBuilder AddVBox()
    {
        var vbox = new VBoxBuilder(fontProvider, iconDrawer, DefaultDefinition);
        children.Add(vbox);
        return vbox;
    }

    public FlexboxBuilder AddFlexbox()
    {
        var flexbox = new FlexboxBuilder(fontProvider, iconDrawer, DefaultDefinition);
        children.Add(flexbox);
        return flexbox;
    }
}