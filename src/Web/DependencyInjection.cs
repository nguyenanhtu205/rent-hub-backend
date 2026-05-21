using Application.Common.Interfaces;
using Web.Services;

namespace Web;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();

        builder.Services.AddCors();

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

        builder.Services.AddJwtAuthentication(builder.Configuration);

        builder.Services.AddCustomRateLimiter();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("Lessor", policy => policy.RequireRole("Lessor"))
            .AddPolicy("Renter", policy => policy.RequireRole("Renter"))
            .AddPolicy("Manager", policy => policy.RequireRole("Manager"))
            .AddPolicy("Surveyor", policy => policy.RequireRole("Surveyor"))
            .AddPolicy("FinanceStaff", policy => policy.RequireRole("FinanceStaff"))
            .AddPolicy("LegalStaff", policy => policy.RequireRole("LegalStaff"))
            .AddPolicy("Broker", policy => policy.RequireRole("Broker"));

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApi(options =>
        {
            options.AddOperationTransformer<ApiExceptionOperationTransformer>();
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
    }
}
