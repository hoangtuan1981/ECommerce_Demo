
# Microsoft cung cấp package:
    dotnet add package Asp.Versioning.Http
    dotnet add package Asp.Versioning.Mvc
    dotnet add package Asp.Versioning.Mvc.ApiExplorer




# Minimal API

    Endpoints
    │
    ├── V1
    │     ProductEndpoint.cs
    │
    ├── V2
    │     ProductEndpoint.cs
    │
    └── EndpointExtensions.cs



    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    1. api/v1

    var v1 = app.MapGroup("/api/v1/products");
    v1.MapGet("/", () =>
    {
        return Results.Ok(new[]
        {
            new { Id = 1, Name = "Laptop", Price = 1500 }
        });
    });

    v1.MapGet("/{id:int}", (int id) =>
    {
        return Results.Ok(new { Id = id, Name = "Laptop", Price = 1500 });
    });

    2. api/v2
    var v2 = app.MapGroup("/api/v2/products");
    v2.MapGet("/", () =>
    {
        return Results.Ok(new[]
        {
            new { Id = 1, Name = "Laptop Pro", Price = 2000, Currency = "USD" }
        });
    });

    v2.MapGet("/{id:int}", (int id) =>
    {
        return Results.Ok(new { Id = id, Name = "Laptop Pro", Price = 2000, Currency = "USD" });
    });

    app.Run();

# Restfull api

    Controllers
    ├── V1
    │   └── ProductController.cs
    │
    ├── V2
    │   └── ProductController.cs


1. Program.cs

    using Asp.Versioning;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1,0);

        options.AssumeDefaultVersionWhenUnspecified = true;

        options.ReportApiVersions = true;

        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.MapControllers();

    app.Run();

2. ProductController V1
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[]
            {
                new
                {
                    Id = 1,
                    Name = "iPhone 15"
                }
            });
        }
    }

2. ProductController V2

    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[]
            {
                new
                {
                    Id = 1,
                    Name = "iPhone 15",
                    Price = 1200,
                    Category = "Phone",
                    Stock = 100
                }
            });
        }
    }