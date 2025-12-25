namespace DisplayUtil.Building;

public class DefaultDefinitionBuilder<TBuilder>(DefaultDefinition? template = null)
    where TBuilder : DefaultDefinitionBuilder<TBuilder>
{
    public DefaultDefinition DefaultDefinition { get; protected set; }
        = template ?? DefaultDefinition.Default;

    public TBuilder WithTextSize(int textSize)
    {
        DefaultDefinition = DefaultDefinition with { TextSize = textSize };
        return (TBuilder)this;
    }

    public TBuilder WithFont(string font)
    {
        DefaultDefinition = DefaultDefinition with { Font = font };
        return (TBuilder)this;
    }

    public TBuilder WithIconHeight(int iconHeight)
    {
        DefaultDefinition = DefaultDefinition with { IconHeight = iconHeight };
        return (TBuilder)this;
    }
}

public class DefaultDefinitionBuilder : DefaultDefinitionBuilder<DefaultDefinitionBuilder>
{
}