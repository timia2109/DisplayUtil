using DisplayUtil.EspUtilities;
using DisplayUtil.HomeAssistant;
using DisplayUtil.MqttExport;
using DisplayUtil.Providers;
using DisplayUtil.Scenes;
using DisplayUtil.Template;
using DisplayUtil.Utils;
using DisplayUtil.XmlModel;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder
    .AddProviders()
    .AddTemplates()
    .AddHassSupport()
    .AddMqttWriter()
    .AddEspUtilities();

builder.Services
    .AddSingleton<XmlLayoutDeserializer>()
    .AddTransient<IconDrawer>()
    .AddQuartz()
    .AddQuartzHostedService(options =>
    {
        options.WaitForJobsToComplete = true;
    });

builder.Services.AddScreenProvider(o => o
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

app.UseEspUtilities();
app.UseStaticFiles();

app.Run();