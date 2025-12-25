using DisplayUtil.Building.Font;
using DisplayUtil.Layouting;
using DisplayUtil.Layouting.Utils;

namespace DisplayUtil.Building.Items;

public class HBoxBuilder(
    IFontProvider fontProvider,
    IIconDrawer iconDrawer,
    DefaultDefinition defaultDefinition)
    : CollectionBuilder<HBoxBuilder>(fontProvider, iconDrawer, defaultDefinition)
{
    public int Gap { get; internal set; }

    public HBoxBuilder WithGap(int gap)
    {
        Gap = gap;
        return this;
    }

    public override Element Build()
    {
        return FillChildren(new HBoxElement(Gap));
    }
}