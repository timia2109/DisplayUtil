using System.Collections.Frozen;
using DisplayUtil.Building.Font;
using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Infrastructure.Providers.Font;

public class FontProviderBuilder
{
    private List<Font> _googleFonts = [];
    private List<Font> _localFonts = [];
    private string? _googleFontDirectory;

    public FontProviderBuilder AddGoogleFont(string name, string path)
    {
        _googleFonts.Add(new Font(name, path));
        return this;
    }

    public FontProviderBuilder AddLocalFont(string name, string path)
    {
        _localFonts.Add(new Font(name, path));
        return this;
    }

    public FontProviderBuilder SetGoogleFontDirectory(string directory)
    {
        _googleFontDirectory = directory;
        return this;
    }

    internal IFontProvider Build(IServiceProvider serviceProvider)
    {
        // Only local fonts. We are done
        if (_googleFonts.Count == 0)
        {
            return new StaticFileFontProvider(
                _localFonts
                    .ToFrozenDictionary(f => f.Name, f => f.Path)
            );
        }

        // Guard if google font directory is not set
        if (string.IsNullOrEmpty(_googleFontDirectory))
        {
            throw new InvalidOperationException("Google font directory is not set");
        }

        var downloader = ActivatorUtilities.CreateInstance<GFontDownloader>(
            serviceProvider,
            _googleFontDirectory
        );

        foreach (var font in _googleFonts)
        {
            var path = downloader.DownloadFont(font.Path);
            _localFonts.Add(new Font(font.Name, path));
        }

        return new StaticFileFontProvider(
            _localFonts.ToFrozenDictionary(f => f.Name, f => f.Path)
        );
    }

    private record Font(string Name, string Path);
}