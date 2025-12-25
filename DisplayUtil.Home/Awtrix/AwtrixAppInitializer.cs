using DisplayUtil.Awtrix;

namespace DisplayUtil.Home.Awtrix;

internal static class AwtrixAppInitializer
{
    public static async Task<WebApplicationBuilder> AddAwtrixAppAsync(this WebApplicationBuilder builder)
    {
        await builder.AddAwtrixAsync(
            "Awtrix"
        );

        return builder;
    }
}