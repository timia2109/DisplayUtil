namespace DisplayUtil.Building;

/// <summary>
///     Setting defaults for the child elements
/// </summary>
public record struct DefaultDefinition
{
    /// <summary>
    ///     Font
    /// </summary>
    public string Font;

    /// <summary>
    ///     Icon Height
    /// </summary>
    public int IconHeight;

    /// <summary>
    ///     Textsize
    /// </summary>
    public int TextSize;

    /// <summary>
    ///     The default set
    ///     TODO: Use config
    /// </summary>
    public static DefaultDefinition Default =>
        new()
        {
            TextSize = 20,
            Font = "Roboto-Medium",
            IconHeight = 20
        };

    /// <summary>
    ///     Overrides the defaults
    /// </summary>
    /// <param name="other">Defined defaults on that Element</param>
    /// <returns>The merged</returns>
    public DefaultDefinition MergeWith(DefaultDefinition? other)
    {
        if (other == null) return this;
        var o = other.Value;

        return new DefaultDefinition
        {
            TextSize = o.TextSize != 0 ? o.TextSize : TextSize,
            Font = o.Font ?? Font,
            IconHeight = o.IconHeight != 0 ? o.IconHeight : IconHeight
        };
    }
}