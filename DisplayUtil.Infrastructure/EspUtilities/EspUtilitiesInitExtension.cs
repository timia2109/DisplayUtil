using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DisplayUtil.Infrastructure.EspUtilities;

public static class EspUtilitiesInitExtension
{
    private const string BinaryMediaType = "application/octet-stream";

    public const string CompressedImageRoute = "/esp/{providerId}",
                        PlainImageRoute = "/esp/bits/{providerId}";

    /// <summary>
    /// Add the ESP Utilities
    /// </summary>
    /// <param name="builder">Application Builder</param>
    /// <returns>Application Builder</returns>
    public static IHostApplicationBuilder AddEspUtilities(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<EspImageProvider>();
        return builder;
    }

    /// <summary>
    /// Maps the ESP Utilities
    /// </summary>
    /// <param name="app">Web App</param>
    /// <returns>Web Application</returns>
    public static WebApplication UseEspUtilities(this WebApplication app)
    {
        app.MapGet(CompressedImageRoute, async (string providerId, HttpContext ctx, EspImageProvider espProvider) =>
        {
            var accept = ctx.Request.Headers.Accept;
            var imageResult = await espProvider.GetAsRunLengthAsync(providerId);

            if (imageResult == null)
                return Results.NotFound();

            var (data, size) = imageResult.Value;
            ctx.Response.Headers.Append("X-Width", size.Width.ToString());
            ctx.Response.Headers.Append("X-Height", size.Height.ToString());

            if (accept.Contains(BinaryMediaType))
            {
                return Results.Bytes(data, BinaryMediaType);
            }

            var base64 = Convert.ToBase64String(data);
            return Results.Text(base64, "text/plain", Encoding.ASCII);
        })
            .WithName("Get ESP Image")
            .WithOpenApi();

        app.MapGet(PlainImageRoute, async (string providerId, HttpContext ctx, EspImageProvider espProvider) =>
        {
            var accept = ctx.Request.Headers.Accept;
            var imageResult = await espProvider.GetAsPlainBytesAsync(providerId);

            if (imageResult == null)
                return Results.NotFound();

            var (data, size) = imageResult.Value;

            ctx.Response.Headers.Append("X-Width", size.Width.ToString());
            ctx.Response.Headers.Append("X-Height", size.Height.ToString());

            if (accept.Contains(BinaryMediaType))
            {
                return Results.Bytes(data, BinaryMediaType);
            }

            var base64 = Convert.ToBase64String(data);
            return Results.Text(base64, "text/plain", Encoding.ASCII);
        })
            .WithName("Get ESP Bit Image")
            .WithOpenApi();

        return app;
    }


}