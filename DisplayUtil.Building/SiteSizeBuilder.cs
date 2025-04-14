using DisplayUtil.Layouting;

namespace DisplayUtil.Building;

public sealed class SiteSizeBuilder
{
    private int _top = 0;
    private int _right = 0;
    private int _bottom = 0;
    private int _left = 0;

    public SiteSizeBuilder WithAll(int all)
    {
        _bottom = _left = _bottom = _left = all;
        return this;
    }

    public SiteSizeBuilder WithX(int x)
    {
        _left = _right = x;
        return this;
    }

    public SiteSizeBuilder WithY(int y)
    {
        _bottom = _top = y;
        return this;
    }

    public SiteSizeBuilder WithTop(int top)
    {
        _top = top;
        return this;
    }

    public SiteSizeBuilder WithLeft(int left)
    {
        _left = left;
        return this;
    }

    public SiteSizeBuilder WithRight(int right)
    {
        _right = right;
        return this;
    }

    public SiteSizeBuilder WithBottom(int bottom)
    {
        _bottom = bottom;
        return this;
    }

    internal SiteSize Build()
    {
        return new SiteSize
        {
            Bottom = _bottom,
            Left = _left,
            Right = _right,
            Top = _top
        };
    }

}