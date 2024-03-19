using System.Collections.Frozen;
using DisplayUtil.Template;

namespace DisplayUtil.Scenes;

public class ScreenRepoBuilder(
    Dictionary<string, Func<IServiceProvider, IScreenProvider>> screenProviders,
    IServiceCollection services
)
{
    public ScreenRepoBuilder Add<TType>(string providerId)
        where TType : class, IScreenProvider
    {
        services.AddSingleton<TType>();
        screenProviders.Add(
            providerId,
            e => (IScreenProvider)e.GetRequiredService(typeof(TType))
        );
        return this;
    }

    public ScreenRepoBuilder AddScribanFiles(string? path = null)
    {
        path ??= @"./Resources/screens";

        var files = Directory.EnumerateFiles(path, "*.sbntxt")
            .Select(p => new { Path = p, Name = Path.GetFileNameWithoutExtension(p) })
            .Select(p => new KeyValuePair<string, Func<IServiceProvider, IScreenProvider>>(
                p.Name,
                e => ActivatorUtilities.CreateInstance<ScribanScreenProvider>(e, [p.Path])
            ));

        foreach (var (k, v) in files)
        {
            screenProviders.Add(k, v);
        }

        return this;
    }
}

public static class ScreenRepoBuilderExtension
{
    public static IServiceCollection AddScreenProvider(
        this IServiceCollection services, Action<ScreenRepoBuilder> action)
    {
        var dictionary = new Dictionary<string, Func<IServiceProvider, IScreenProvider>>();
        var builder = new ScreenRepoBuilder(dictionary, services);
        action(builder);

        services.AddSingleton(o =>
        {
            var types = dictionary
                .ToDictionary(k => k.Key, v => v.Value(o))
                .ToFrozenDictionary();

            return new ScreenRepository(types);
        });
        return services;
    }

}