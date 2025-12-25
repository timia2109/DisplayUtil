using System.Globalization;
using DisplayUtil.Building;
using DisplayUtil.Building.Items;
using DisplayUtil.Home.HomeAssistant;
using DisplayUtil.Home.Utils;
using DisplayUtil.Infrastructure.Providers.Image;
using DisplayUtil.Layouting;
using DisplayUtil.Widgets;
using SkiaSharp;

namespace DisplayUtil.Home.Screens;

public sealed class DefaultScreen(
    ScreenBuilder screenBuilder,
    HassUtil hassUtil,
    IWidgetService widgetService
) : ISingleImageProvider
{
    public async ValueTask<SKBitmap> GetImageAsync()
    {
        var builder = screenBuilder
            .WithHeight(800)
            .WithWidth(480);

        var flexbox = builder.UseFlexbox()
            .WithDirection(FlexDirection.Vertical)
            .WithJustifyContent(JustifyContent.Between)
            .AddVBox(RenderHead);

        var widgets = widgetService.GetActiveWidgets();
        foreach (var widget in widgets)
        {
            var widgetBuilder = flexbox.WithElement(await widget.RenderAsync());
        }

        return builder.Draw();
    }

    private void RenderHead(VBoxBuilder vBoxBuilder)
    {
        vBoxBuilder
            .AddFlexbox(RenderTimeRow);
    }

    private void RenderTimeRow(FlexboxBuilder flexboxBuilder)
    {
        var now = DateTime.Now;

        flexboxBuilder
            .WithAlignItems(AlignItems.Center)
            .WithJustifyContent(JustifyContent.Between)
            .WithPadding(p => p.WithAll(2))
            .WithMargin(p => p.WithBottom(2))
            .WithBorder(p => p.WithBottom(2))
            .AddVBox(v =>
                v.WithIconElement("fa:calendar",
                        now.ToString("dddd", CultureInfo.CurrentCulture)
                    )
                    .WithText(now.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture))
            )
            .WithText(now.ToString("t"), "ProductSansBold", 80);
    }
}