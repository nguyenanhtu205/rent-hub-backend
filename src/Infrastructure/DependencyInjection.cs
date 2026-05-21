using Ardalis.GuardClauses;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using Supabase;
using SupabaseOptions = Infrastructure.Options.SupabaseOptions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Guard.Against.Null(connectionString, message: "Connection string not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        NpgsqlDataSourceBuilder dataSourceBuilder = new(connectionString);
        dataSourceBuilder.EnableDynamicJson();

        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(dataSource);
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddScoped<IJwtProvider, JwtProvider>();

        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

        builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

        builder.Services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();

        builder.Services.Configure<SupabaseOptions>(builder.Configuration.GetSection(SupabaseOptions.SectionName));

        builder.Services.AddSingleton<Client>(sp =>
        {
            SupabaseOptions options = sp.GetRequiredService<IOptions<SupabaseOptions>>().Value;
            Client client = new(options.Url, options.Key);
            client.InitializeAsync().GetAwaiter().GetResult();
            return client;
        });

        builder.Services.AddScoped<IStorageService, SupabaseStorageService>();
    }
}
