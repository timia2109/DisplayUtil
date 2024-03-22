using System.Collections.Frozen;
using DisplayUtil.Template;

namespace DisplayUtil.Scenes;

public class ScreenRepoBuilder(
    Dictionary<string, Type> staticScreenProviderTypes,
    IServiceCollection services
)
{
    public ScreenRepoBuilder AddSingleton<TType>(string providerId)
        where TType : class, IScreenProvider
    {
        staticScreenProviderTypes.Add(providerId, typeof(TType));
        services.AddSingleton<TType>();
        return this;
    }

    public ScreenRepoBuilder AddScoped<TType>(string providerId)
        where TType : class, IScreenProvider
    {
        staticScreenProviderTypes.Add(providerId, typeof(TType));
        services.AddScoped<TType>();
        return this;
    }

    public ScreenRepoBuilder AddScribanFiles(string? path = null)
    {
        services.AddScoped<IScreenProviderSource, ScibanScreenProviderSource>();
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

        // Add static
        if (dictionary.Count != 0)
        {
            services.AddSingleton<IScreenProviderSource>(s => new StaticScreenProviderSource(
                s, dictionary.ToFrozenDictionary()
            ));
        }

        services.AddScoped<ScreenRepository>();

        return services;
    }

}