using Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

WebApplication app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.UseExceptionHandler();

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.MapOpenApi();

app.Map("/", () => Results.Redirect("/scalar"));

app.Run();
