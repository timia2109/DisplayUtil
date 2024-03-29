using System.Text;
using DisplayUtil;
using DisplayUtil.MqttExport;
using DisplayUtil.Scenes;
using DisplayUtil.Serializing;
using DisplayUtil.Template;
using DisplayUtil.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddHassSupport()
    .AddMqttWriter();

builder.Services.AddSingleton(FontProvider.Create())
    .AddSingleton<XmlLayoutDeserializer>()
    .AddSingleton<TemplateLoader>();

builder.Services.AddScoped<TemplateRenderer>()
    .AddScoped<TemplateContextProvider>();

builder.Services.AddTransient<FaIconDrawer>();

builder.Services.AddScreenProvider(o => o
    .AddSingleton<TestProvider>("test")
    .AddSingleton<TestLayoutProvider>("layout")
    .AddSingleton<TestFontSizeProvider>("testFont")
    .AddScribanFiles()
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/preview/{providerId}", async (string providerId, ScreenRepository repo) =>
{
    using var image = await repo.GetImageAsync(providerId);
    using var data = image.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
    return Results.File(data.ToArray(), "image/png");

})
.WithName("Preview Image")
.WithOpenApi();

app.MapPost("/publish/{providerId}", async (string providerId, MqttExporter exporter) =>
{
    await exporter.ExportScreenToMqtt(providerId);
    GC.Collect();
    return Results.NoContent();
})
.WithName("Publish manual to MQTT")
.WithOpenApi();

app.MapGet("/esp/{providerId}", async (string providerId, EspImageProvider espProvider) =>
{
    var data = await espProvider.GetAsRunLengthAsync(providerId);
    var base64 = Convert.ToBase64String(data);
    return Results.Text(base64, "text/plain", Encoding.ASCII);
})
.WithName("Get ESP Image")
.WithOpenApi();

app.Run();