using DisplayUtil.Building;
using DisplayUtil.Infrastructure.Providers.Image;
using DisplayUtil.Layouting;
using SkiaSharp;

namespace DisplayUtil.Home.Screens;

public sealed class ExampleScreen(ScreenBuilder screenBuilder) : ISingleImageProvider
{
    public ValueTask<SKBitmap> GetImageAsync()
    {
        var builder = screenBuilder
            .WithWidth(800)
            .WithHeight(480);

        builder.UseFlexbox()
            .WithDirection(FlexDirection.Vertical)
            .WithJustifyContent(JustifyContent.Between)
            .WithIcon("fa:home", 80)
            .WithText("Hello World!")
            .WithText("This is an example screen");

        return ValueTask.FromResult(builder.Draw());
    }
}