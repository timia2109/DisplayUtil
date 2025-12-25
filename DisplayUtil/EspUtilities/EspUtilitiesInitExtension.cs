using System.Text;

namespace DisplayUtil.EspUtilities;

public static class EspUtilitiesInitExtension
{
    private const string BinaryMediaType = "application/octet-stream";

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
                var accept = ctx.Request.Headers.Accept;
                var (data, size) = await espProvider.GetAsRunLengthAsync(providerId);
                ctx.Response.Headers.Append("X-Width", size.Width.ToString());
                ctx.Response.Headers.Append("X-Height", size.Height.ToString());

                if (accept.Contains(BinaryMediaType)) return Results.Bytes(data, BinaryMediaType);

                var base64 = Convert.ToBase64String(data);
                return Results.Text(base64, "text/plain", Encoding.ASCII);
            })
            .WithName("Get ESP Image")
            .WithOpenApi();

        app.MapGet(PlainImageRoute, async (string providerId, HttpContext ctx, EspImageProvider espProvider) =>
            {
                var accept = ctx.Request.Headers.Accept;
                var (data, size) = await espProvider.GetAsPlainBytesAsync(providerId);
                ctx.Response.Headers.Append("X-Width", size.Width.ToString());
                ctx.Response.Headers.Append("X-Height", size.Height.ToString());

                if (accept.Contains(BinaryMediaType)) return Results.Bytes(data, BinaryMediaType);

                var base64 = Convert.ToBase64String(data);
                return Results.Text(base64, "text/plain", Encoding.ASCII);
            })
            .WithName("Get ESP Bit Image")
            .WithOpenApi();

        return app;
    }
}