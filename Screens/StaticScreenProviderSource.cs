namespace DisplayUtil.Scenes;

internal class StaticScreenProviderSource(
    IServiceProvider serviceProvider,
    IReadOnlyDictionary<string, Type> typeList
) : IScreenProviderSource
{
    public IScreenProvider? GetScreenProvider(string id)
    {
        if (!typeList.TryGetValue(id, out var type)) return null;

        return (IScreenProvider)serviceProvider.GetRequiredService(type);
    }
}