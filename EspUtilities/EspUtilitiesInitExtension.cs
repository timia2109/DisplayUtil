namespace DisplayUtil.EspUtilities;

public static class EspUtilitiesInitExtension
{

    public static IHostApplicationBuilder AddEspUtilities(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<EspImageProvider>();
        return builder;
    }


}