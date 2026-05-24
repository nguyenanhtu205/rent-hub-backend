using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseCors(static builder =>
    builder
        .SetIsOriginAllowed(origin =>
            origin == "http://localhost:5173" ||
            origin == "https://rent-hub-hanoi.vercel.app" ||
            origin.EndsWith(".vercel.app")
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
);

app.UseExceptionHandler();

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.MapOpenApi();

app.MapScalarApiReference();

app.Map("/", () => Results.Redirect("/scalar"));

app.MapEndpoints(typeof(Program).Assembly);

app.Run();
