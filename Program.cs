using System.Text;
using DisplayUtil;
using DisplayUtil.EspUtilities;
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
    .AddMqttWriter()
    .AddEspUtilities();

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
    return Results.NoContent();
})
.WithName("Publish manual to MQTT")
.WithOpenApi();

app.MapGet("/esp/{providerId}", async (string providerId, HttpContext ctx, EspImageProvider espProvider) =>
{
    var (data, size) = await espProvider.GetAsRunLengthAsync(providerId);
    var base64 = Convert.ToBase64String(data);
    ctx.Response.Headers.Append("X-Width", size.Width.ToString());
    ctx.Response.Headers.Append("X-Height", size.Height.ToString());
    return Results.Text(base64, "text/plain", Encoding.ASCII);
})
.WithName("Get ESP Image")
.WithOpenApi();

app.MapGet("/esp/bits/{providerId}", async (string providerId, HttpContext ctx, EspImageProvider espProvider) =>
{
    var (data, size) = await espProvider.GetAsPlainBytesAsync(providerId);
    var base64 = Convert.ToBase64String(data);
    ctx.Response.Headers.Append("X-Width", size.Width.ToString());
    ctx.Response.Headers.Append("X-Height", size.Height.ToString());
    return Results.Text(base64, "text/plain", Encoding.ASCII);
})
.WithName("Get ESP Bit Image")
.WithOpenApi();

app.UseStaticFiles();

app.Run();