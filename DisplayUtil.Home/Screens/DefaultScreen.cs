using DisplayUtil.Building;
using DisplayUtil.Building.Items;
using DisplayUtil.Home.HomeAssistant;
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
            .WithPadding(p => p.WithBottom(2))
            .WithMargin(p => p.WithBottom(2))
            .WithBorder(p => p.WithBottom(2))
            .AddVBox(v =>
                v.AddHBox(h => h
                    .WithIcon("fa:calendar")
                    .WithText(
                        now.ToString("dd")
                    )
                    .WithText(now.ToString("dd.MM.yyyy"))
                )
            )
            .WithText(now.ToString("HH:MM"), "ProductSansBold", 80);
    }
}