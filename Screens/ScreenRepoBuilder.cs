using System.Collections.Frozen;

namespace DisplayUtil.Scenes;

public class ScreenRepoBuilder(
    Dictionary<string, Type> screenProviders,
    IServiceCollection services
)
{

    public ScreenRepoBuilder Add<TType>(string providerId)
        where TType : class, IScreenProvider
    {
        services.AddSingleton<TType>();
        screenProviders.Add(providerId, typeof(TType));
        return this;
    }
}

public static class ScreenRepoBuilderExtension
{
    public static IServiceCollection AddScreenProvider(
        this IServiceCollection services, Action<ScreenRepoBuilder> action)
    {
        var dictionary = new Dictionary<string, Type>();
        var builder = new ScreenRepoBuilder(dictionary, services);
        action(builder);

        var types = dictionary.ToFrozenDictionary();
        services.AddSingleton(o => new ScreenRepository(types, o));
        return services;
    }

}