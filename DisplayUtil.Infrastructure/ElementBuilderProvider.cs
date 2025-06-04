using DisplayUtil.Building;
using DisplayUtil.Building.Font;
using DisplayUtil.Building.Items;
using DisplayUtil.Infrastructure.Providers;
using DisplayUtil.Layouting.Utils;

namespace DisplayUtil.Infrastructure;

public sealed class ElementBuilderProvider(
    IFontProvider fontProvider,
    IIconDrawer iconDrawer,
    DefaultDefinitionProvider defaultDefinitionProvider
)
{
    public ScreenBuilder ScreenBuilder => new(fontProvider,
        iconDrawer, defaultDefinitionProvider.Definition);

    public VBoxBuilder VBoxBuilder => new(fontProvider,
        iconDrawer, defaultDefinitionProvider.Definition);

    public HBoxBuilder HBoxBuilder => new(fontProvider,
        iconDrawer, defaultDefinitionProvider.Definition);

    public FlexboxBuilder FlexboxBuilder => new(fontProvider,
        iconDrawer, defaultDefinitionProvider.Definition);
}