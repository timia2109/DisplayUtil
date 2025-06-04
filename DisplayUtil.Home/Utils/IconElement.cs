using DisplayUtil.Building;

namespace DisplayUtil.Home.Utils;

public static class IconElement
{
    public static CollectionBuilder<TBuilder> WithIconElement<TBuilder>(
        this CollectionBuilder<TBuilder> builder,
        string iconName,
        string? content
    ) where TBuilder : CollectionBuilder<TBuilder>
    {
        if (string.IsNullOrWhiteSpace(content))
            return builder;

        var hBox = builder.AddHBox()
            .WithGap(5);

        hBox.WithIcon(iconName);
        hBox.WithText(content);

        return builder;
    }
}