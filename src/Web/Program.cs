WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
