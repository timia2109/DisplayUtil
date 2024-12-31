using DisplayUtil.Home.Screens;
using DisplayUtil.Infrastructure;
using DisplayUtil.Infrastructure.EspUtilities;
using DisplayUtil.Infrastructure.Providers.Font;
using DisplayUtil.Infrastructure.Providers.Icons;
using DisplayUtil.Infrastructure.Providers.Image;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);

builder.Services
    .AddOpenApi()
    .AddHttpClient()
    .AddEspUtilities()
    .AddScreenBuilder(d => d
        .WithTextSize(45)
        .WithFont("Roboto")
        .WithIconHeight(45)
    )
    .AddFontProvider(builder.Configuration.GetSection("Fonts"))
    .AddFolderIconProvider(builder.Configuration);

builder.Services
    .AddSingleImageProvider<ExampleScreen>("example");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapImagePreview();
app.UseEspUtilities();

app.Run();