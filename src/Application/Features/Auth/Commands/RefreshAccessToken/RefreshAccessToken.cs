namespace Application.Features.Auth.Commands.RefreshAccessToken;

public record RefreshAccessTokenResponse(string AccessToken, string RefreshToken);

public record RefreshAccessTokenCommand(string RefreshToken) : IRequest<RefreshAccessTokenResponse>;

public class RefreshAccessTokenCommandHandler(
    IApplicationDbContext context,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenHasher refreshTokenHasher,
    IJwtProvider jwtProvider)
    : IRequestHandler<RefreshAccessTokenCommand, RefreshAccessTokenResponse>
{
    public async Task<RefreshAccessTokenResponse> Handle(RefreshAccessTokenCommand request,
        CancellationToken cancellationToken)
    {
        string token = refreshTokenHasher.Hash(request.RefreshToken);

        RefreshToken? refreshToken = await context.RefreshTokens
            .Include(x => x.Account!)
            .ThenInclude(x => x.Staff)
            .Include(x => x.Account!)
            .ThenInclude(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (refreshToken == null)
        {
            throw new NotFoundException("Refresh token does not exist");
        }

        if (refreshToken.IsRevoked)
        {
            throw new UnauthorizedAccessException("Refresh token is revoked");
        }

        if (refreshToken.ExpiredAt < DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token is expired");
        }

        Account? account = refreshToken.Account;

        if (account == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        int userId;
        string role;
        string name;
        string phone;

        if (account.Role == AccountRole.Staff)
        {
            if (account.Staff == null)
            {
                throw new NotFoundException("Staff profile not found");
            }

            userId = account.Staff.Id;
            role = account.Staff.Role.ToString();
            name = account.Staff.Name;
            phone = account.Staff.Phone;
        }
        else
        {
            if (account.Customer == null)
            {
                throw new NotFoundException("Customer profile not found");
            }

            userId = account.Customer.Id;
            role = account.Customer.Type.ToString();
            name = account.Customer.Name;
            phone = account.Customer.Phone;
        }

        string newRawRefreshToken = refreshTokenGenerator.Generate();

        RefreshToken newRefreshToken = new()
        {
            Token = refreshTokenHasher.Hash(newRawRefreshToken),
            AccountId = account.Id,
            ExpiredAt = DateTimeOffset.UtcNow.AddDays(7),
            IsRevoked = false
        };

        refreshToken.IsRevoked = true;
        refreshToken.ReplacedByToken = newRefreshToken;

        context.RefreshTokens.Add(newRefreshToken);
        await context.SaveChangesAsync(cancellationToken);

        string accessToken = jwtProvider.Generate(userId, account.Id, role, name, phone, account.Email);

        return new RefreshAccessTokenResponse(accessToken, newRawRefreshToken);
    }
}
