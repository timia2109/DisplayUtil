using System.Text;

namespace DisplayUtil.EspUtilities;

public static class EspUtilitiesInitExtension
{
    public const string CompressedImageRoute = "/esp/{providerId}",
                        PlainImageRoute = "/esp/bits/{providerId}";

    public static IHostApplicationBuilder AddEspUtilities(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<EspImageProvider>();
        return builder;
    }

    public static WebApplication UseEspUtilities(this WebApplication app)
    {
        app.MapGet(CompressedImageRoute, async (string providerId, HttpContext ctx, EspImageProvider espProvider) =>
        {
            var (data, size) = await espProvider.GetAsRunLengthAsync(providerId);
            var base64 = Convert.ToBase64String(data);
            ctx.Response.Headers.Append("X-Width", size.Width.ToString());
            ctx.Response.Headers.Append("X-Height", size.Height.ToString());
            return Results.Text(base64, "text/plain", Encoding.ASCII);
        })
            .WithName("Get ESP Image")
            .WithOpenApi();

        app.MapGet(PlainImageRoute, async (string providerId, HttpContext ctx, EspImageProvider espProvider) =>
        {
            var (data, size) = await espProvider.GetAsPlainBytesAsync(providerId);
            var base64 = Convert.ToBase64String(data);
            ctx.Response.Headers.Append("X-Width", size.Width.ToString());
            ctx.Response.Headers.Append("X-Height", size.Height.ToString());
            return Results.Text(base64, "text/plain", Encoding.ASCII);
        })
            .WithName("Get ESP Bit Image")
            .WithOpenApi();

        return app;
    }


}