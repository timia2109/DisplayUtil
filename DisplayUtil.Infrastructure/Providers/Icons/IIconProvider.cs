namespace DisplayUtil.Infrastructure.Providers.Icons;

/// <summary>
///     Interface for providing SVG icon pathes.
/// </summary>
public interface IIconProvider
{
    /// <summary>
    ///     Checks if the icon can be resolved.
    /// </summary>
    /// <param name="iconName">Name of icon</param>
    /// <returns>Can resolve?</returns>
    bool CanResolve(string iconName);

    /// <summary>
    ///     Get the SVG icon
    /// </summary>
    /// <param name="iconName">Name of icon</param>
    /// <returns>Icon Stream</returns>
    Stream GetSvgIcon(string iconName);
}