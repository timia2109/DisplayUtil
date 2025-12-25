using DisplayUtil.Building.Font;
using DisplayUtil.Layouting;
using DisplayUtil.Layouting.Utils;

namespace DisplayUtil.Building.Items;

public class FlexboxBuilder(IFontProvider fontProvider, IIconDrawer iconDrawer, DefaultDefinition defaultDefinition)
    : CollectionBuilder<FlexboxBuilder>(fontProvider, iconDrawer, defaultDefinition)
{
    private AlignItems _alignItems = AlignItems.Start;
    private FlexDirection _flexDirection = FlexDirection.Horizontal;
    private int _gap;
    private JustifyContent _justifyContent = JustifyContent.Start;

    public FlexboxBuilder WithDirection(FlexDirection direction)
    {
        _flexDirection = direction;
        return this;
    }

    public FlexboxBuilder WithGap(int gap)
    {
        _gap = gap;
        return this;
    }

    public FlexboxBuilder WithJustifyContent(JustifyContent justifyContent)
    {
        _justifyContent = justifyContent;
        return this;
    }

    public FlexboxBuilder WithAlignItems(AlignItems alignItems)
    {
        _alignItems = alignItems;
        return this;
    }

    public override Element Build()
    {
        return FillChildren(new FlexboxElement(
            _gap, _flexDirection, _justifyContent, _alignItems));
    }
}