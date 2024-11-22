namespace DisplayUtil.Layouting;

/// <summary>
/// Represents Padding Information
/// </summary>
/// <param name="Top">Top Padding</param>
/// <param name="Right">Right Padding</param>
/// <param name="Bottom">Bottom Padding</param>
/// <param name="Left">Left Padding</param>
public record struct SiteSize(
    int Top = 0,
    int Right = 0,
    int Bottom = 0,
    int Left = 0
)
{
    public SiteSize(int allSizes) : this(allSizes, allSizes, allSizes, allSizes) { }

    public SiteSize(int x, int y) : this(y, x, y, x) { }
}
