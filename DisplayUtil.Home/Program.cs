using DisplayUtil.Home.Awtrix;
using DisplayUtil.Home.HomeAssistant;
using DisplayUtil.Home.Screens;
using DisplayUtil.Home.Utils;
using DisplayUtil.Home.Widgets;
using DisplayUtil.Infrastructure;
using DisplayUtil.Infrastructure.EspUtilities;
using DisplayUtil.Infrastructure.Providers.Font;
using DisplayUtil.Infrastructure.Providers.Icons;
using DisplayUtil.Infrastructure.Providers.Image;
using DisplayUtil.Widgets;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", true);

builder.AddHassSupport();
builder.AddMediaAppIcons();
await builder.AddAwtrixAppAsync();

builder.Services
    .AddOpenApi()
    .AddHttpClient()
    .AddEspUtilities()
    .AddWidgets(w =>
    {
        // TODO: Use Configuration to load media players
        string[] mediaPlayers =
        [
            "media_player.wohnzimmer_2",
            "media_player.googlehome5731"
        ];

        foreach (var mediaPlayer in mediaPlayers)
            w.RegisterWidget(
                mediaPlayer,
                (s, k) => ActivatorUtilities.CreateInstance<MediaPlayerWidget>(
                    s, mediaPlayer),
                100,
                "Media Players"
            );
    })
    .AddScreenBuilder(d => d
        .WithTextSize(45)
        .WithFont("Roboto")
        .WithIconHeight(45)
    )
    .AddFontProvider(builder.Configuration.GetSection("Fonts"))
    .AddFolderIconProvider(builder.Configuration);

builder.Services
    .AddSingleImageProvider<ExampleScreen>("example")
    .AddSingleImageProvider<DefaultScreen>("default");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/swagger", () => Results.Redirect("/scalar/v1"));

app.UseHttpsRedirection();

app.MapImagePreview();
app.UseEspUtilities();

app.Run();