using Identity.API.Endpoints;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Application
builder.Services.AddApplication();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Authentication
builder.Services.AddAuthentication();
//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//    });

builder.Services.AddAuthorization();

// OpenAPI
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Move to Application project
// MediatR
//builder.Services.AddMediatR(cfg =>
//{
//    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
//});
//builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();
// TODO
//builder.Services.AddDbContext<IdentityDbContext>(options =>
//{
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("IdentityDb"));
//});
using (var scope = app.Services.CreateScope())
{
    var seeder =
        scope.ServiceProvider
            .GetRequiredService<DataSeeder>();

    await seeder.SeedAsync();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapIdentityEndpoints();

app.Run();
