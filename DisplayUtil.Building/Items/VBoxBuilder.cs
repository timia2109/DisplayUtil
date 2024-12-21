using DisplayUtil.Building.Font;
using DisplayUtil.Layouting;
using DisplayUtil.Layouting.Utils;

namespace DisplayUtil.Building.Items;

public class VBoxBuilder(
    IFontProvider fontProvider,
    IIconDrawer iconDrawer,
    DefaultDefinition defaultDefinition)
: CollectionBuilder<VBoxBuilder>(fontProvider, iconDrawer, defaultDefinition)
{
    public int Gap { get; internal set; }

    public VBoxBuilder WithGap(int gap)
    {
        Gap = gap;
        return this;
    }

    public override Element Build()
    {
        return FillChildren(new VBoxElement(Gap));
    }
}