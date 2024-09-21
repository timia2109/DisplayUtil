namespace DisplayUtil.Utils;

public static class ServiceCollectionExtension
{

    public static TModel? ConfigureAndGet<TModel>(this IHostApplicationBuilder builder,
        string sectionName)
    where TModel : class
    {
        var section = builder.Configuration.GetSection(sectionName);
        builder.Services.AddOptions<TModel>()
            .Bind(section)
            .ValidateDataAnnotations();

        return section.Get<TModel>();
    }

}