using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout;
using Application.Features.Auth.Commands.RefreshAccessToken;
using Application.Features.Auth.Commands.Register;

namespace Web.Endpoints;

public record AccessTokenResponse(string AccessToken);

public class Auth : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(Register, "register")
            .Produces<AccessTokenResponse>()
            .Produces(StatusCodes.Status409Conflict)
            .RequireRateLimiting("post");

        groupBuilder.MapPost(Login, "login")
            .Produces<AccessTokenResponse>()
            .RequireRateLimiting("post");

        groupBuilder.MapPost(Logout, "logout")
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapPost(RefreshAccessToken, "refresh-token")
            .Produces<AccessTokenResponse>()
            .RequireRateLimiting("post");
    }

    private static CookieOptions GetRefreshTokenCookieOptions(IWebHostEnvironment env)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = env.IsProduction(),
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };
    }

    [EndpointSummary("Register")]
    [EndpointDescription("Create a new user account.")]
    public static async Task<IResult> Register(RegisterCommand command, ISender sender,
        HttpContext httpContext, IWebHostEnvironment env, CancellationToken cancellationToken)
    {
        RegisterResponse result = await sender.Send(command, cancellationToken);

        httpContext.Response.Cookies.Append("refreshToken", result.RefreshToken, GetRefreshTokenCookieOptions(env));

        return Results.Ok(new { result.AccessToken });
    }

    [EndpointSummary("Login")]
    public static async Task<IResult> Login(LoginCommand command, ISender sender,
        HttpContext httpContext, IWebHostEnvironment env, CancellationToken cancellationToken)
    {
        LoginResponse result = await sender.Send(command, cancellationToken);

        httpContext.Response.Cookies.Append("refreshToken", result.RefreshToken, GetRefreshTokenCookieOptions(env));

        return Results.Ok(new { result.AccessToken });
    }

    [EndpointSummary("Logout")]
    public static async Task<IResult> Logout(ISender sender,
        HttpContext httpContext, CancellationToken cancellationToken)
    {
        await sender.Send(new LogoutCommand(), cancellationToken);

        httpContext.Response.Cookies.Delete("refreshToken");

        return Results.NoContent();
    }

    [EndpointSummary("Refresh token")]
    [EndpointDescription("Returns a new access token using a valid refresh token.")]
    public static async Task<IResult> RefreshAccessToken(ISender sender,
        HttpContext httpContext, IWebHostEnvironment env, CancellationToken cancellationToken)
    {
        string? refreshToken = httpContext.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Results.Unauthorized();
        }

        RefreshAccessTokenResponse result = await sender.Send(
            new RefreshAccessTokenCommand(refreshToken), cancellationToken);

        httpContext.Response.Cookies.Append("refreshToken", result.RefreshToken, GetRefreshTokenCookieOptions(env));

        return Results.Ok(new { result.AccessToken });
    }
}
