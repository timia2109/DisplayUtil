using DisplayUtil;
using DisplayUtil.Scenes;
using DisplayUtil.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<FaIconDrawer>();
builder.Services.AddScreenProvider(o => o
    .Add<TestProvider>("test")
    .Add<TestLayoutProvider>("layout")
);

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
    var data = image.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
    return Results.File(data.ToArray(), "image/png");

})
.WithName("Preview Image")
.WithOpenApi();

app.Run();