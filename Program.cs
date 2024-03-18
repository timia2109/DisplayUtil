using DisplayUtil;
using DisplayUtil.Scenes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScreenProvider(o => o.Add<TestProvider>("test"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/preview/{providerId}", async (string providerId, ScreenRepository repo) =>
{
    var image = await repo.GetImageAsync(providerId);
    var data = image.Encode(SkiaSharp.SKEncodedImageFormat.Jpeg, 100);
    return Results.File(data.AsStream(), "image/jpeg");

})
.WithName("Preview Image")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
