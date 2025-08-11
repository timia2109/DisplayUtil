using System.Collections.Frozen;
using Microsoft.Extensions.Options;

namespace DisplayUtil.Home.Utils;

public record MappedIcon
{
    public string AppName { get; init; } = null!;
    public string Icon { get; init; } = null!;
}

public record MappedIcons
{
    public MappedIcon[] Icons { get; init; } = null!;
    public string FallbackIcon { get; init; } = null!;
}

public sealed class MediaAppIconHelper(IOptions<MappedIcons> options)
{
    private readonly IReadOnlyDictionary<string, string> _iconDict
        = options.Value.Icons.ToFrozenDictionary(
            i => i.AppName.ToLower(), i => i.Icon);

    private readonly string _fallbackIcon = options.Value.FallbackIcon;

    public string GetIconForApp(string app)
    {
        var appName = app.ToLower();
        if (!_iconDict.TryGetValue(appName, out var icon))
            return _fallbackIcon;

        return icon;
    }
}

public static class MediaAppIconHelperExtensions
{
    public static WebApplicationBuilder AddMediaAppIcons(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.Configure<MappedIcons>(
            builder.Configuration.GetSection("MediaAppIcons")
        )
        .AddSingleton<MediaAppIconHelper>();

        return builder;
    }
}