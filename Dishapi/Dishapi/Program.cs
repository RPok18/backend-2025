using Dishapi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in all environments (remove if you only want it in Development)
app.UseSwagger();
app.UseSwaggerUI();

// Optional: comment this if you don't want HTTPS redirection locally
// app.UseHttpsRedirection(); // Try commenting this out for local testing

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Starting application...");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
app.MapGet("/", () => Results.Redirect("/api/dishes"));

app.Run();