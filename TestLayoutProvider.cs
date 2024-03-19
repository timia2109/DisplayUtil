using System.Globalization;
using DisplayUtil.Layouting;
using DisplayUtil.Scenes;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil;

internal class TestLayoutProvider(FaIconDrawer iconDrawer) : IScreenProvider
{
    public Task<SKBitmap> GetImageAsync()
    {
        var productSans = SKTypeface.FromFile("./Resources/fonts/ProductSansRegular.ttf");

        var testPaint = new SKPaint
        {
            IsAntialias = true,
            TextSize = 32,
            TextAlign = SKTextAlign.Left,
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            Typeface = productSans
        };

        var now = DateTime.Now;
        var locale = CultureInfo.GetCultureInfo("de-DE");

        var mainFlex = new FlexboxElement(0, FlexDirection.Horizontal, JustifyContent.Between);

        var dateBox = new VBoxElement(2);
        dateBox.Append(
            new HBoxElement(10)
                .Append(new IconElement("calendar", 32, iconDrawer))
                .Append(new TextElement($"{now.ToString("dddd", locale)}, ", testPaint))
        )
        .Append(new TextElement(now.ToString("d", locale), testPaint));
        mainFlex.Append(dateBox);

        var clockPaint = testPaint.Clone();
        clockPaint.Typeface = SKTypeface.FromFile("./Resources/fonts/ProductSansBold.ttf");
        clockPaint.TextSize = 66;
        mainFlex.Append(
            new TextElement(now.ToString("t", locale), clockPaint)
        );

        return Task.FromResult(
            DrawManager.Draw(
                Constants.EPaperDisplaySize,
                mainFlex
            )
        );

        /*var headerBox = new BorderElement(
            new Padding(Bottom: 2),
            new PaddingElement(new Padding(Bottom: 2), mainFlex)
        );

        return Task.FromResult(
            DrawManager.Draw(
                Constants.EPaperDisplaySize,
                new PaddingElement(
                    new Padding(20),
                    headerBox
                )
            )
        );*/
    }
}