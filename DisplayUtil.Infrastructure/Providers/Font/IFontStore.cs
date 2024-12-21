namespace DisplayUtil.Infrastructure.Providers.Font;

public interface IFontStore
{
    bool CanResolveFont(string fontName);
}
