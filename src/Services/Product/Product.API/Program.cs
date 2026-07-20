using Product.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Product.Application;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Application
builder.Services.AddApplication();
// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
// 2. Create Version Set for Minimal API
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .ReportApiVersions()
    .Build();

//builder.Services.AddDbContext<ProductDbContext>(options =>
//{
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("ProductDb"));
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

#region "Temp API"
//gateway
//https://localhost:7016/api/v1/product/GetWeatherForecast

//product
//https://localhost:7002/api/v1/GetWeatherForecast

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//var v1 = app.MapGroup("/api/v1/GetWeatherForecast");
//v1.MapGet("/", () =>
//{
//var forecast = Enumerable.Range(1, 5).Select(index =>
//    new WeatherForecast
//    (
//        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//        Random.Shared.Next(-20, 55),
//        summaries[Random.Shared.Next(summaries.Length)]
//    ))
//    .ToArray();
//return forecast;
//})
//.WithName("GetWeatherForecast");

//app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}

#endregion "Temp API"