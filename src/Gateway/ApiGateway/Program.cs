var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//use Yarp
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//Fix Cors exception when ReactJS App(http://localhost:5173) call service.
builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // Domain của ReactJS Client
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();// Bắt buộc: Cho phép gửi và nhận Cookie chéo domain
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
/* giai thich UseHttpsRedirection
 * FE call http://localhost:5175/api/identity/auth/login
 * sau đó Gateway thực hiện: 307 Temporary Redirect
 * sang HTTPS do: app.UseHttpsRedirection();
 */
//app.UseHttpsRedirection();


app.UseCors("GatewayCors");

//app.UseRouting();

//use Yarp
app.MapReverseProxy();

app.Run();