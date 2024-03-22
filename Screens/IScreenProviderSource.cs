namespace DisplayUtil.Scenes;

/// <summary>
/// Provides <see cref="IScreenProvider"> by Ids
/// </summary>
public interface IScreenProviderSource
{
    /// <summary>
    /// Returns the <see cref="IScreenProvider"> if this source contains it
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Screen Provider If found</returns>
    public IScreenProvider? GetScreenProvider(string id);

}