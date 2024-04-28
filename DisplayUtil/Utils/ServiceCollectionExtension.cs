namespace DisplayUtil.Utils;

public static class ServiceCollectionExtension
{

    public static TModel? ConfigureAndGet<TModel>(this IHostApplicationBuilder builder,
        string sectionName)
    where TModel : class
    {
        var section = builder.Configuration.GetSection(sectionName);
        builder.Services.Configure<TModel>(section);

        return section.Get<TModel>();
    }

}