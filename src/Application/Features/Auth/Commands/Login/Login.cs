namespace Application.Features.Auth.Commands.Login;

public record LoginResponse(string AccessToken, string RefreshToken);

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

public class LoginCommandHandler(
    IApplicationDbContext context,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenHasher refreshTokenHasher)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Account? account = await context.Accounts
            .Include(x => x.Staff)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (account is null)
        {
            throw new NotFoundException($"Email {request.Email} does not exist");
        }

        if (account.Status != AccountStatus.Active)
        {
            throw new UnauthorizedAccessException("Account is not active");
        }

        if (!passwordHasher.Verify(request.Password, account.Password))
        {
            throw new UnauthorizedAccessException("Invalid password");
        }

        int userId;
        string role;
        string name;
        string phone;

        if (account.Role == AccountRole.Staff)
        {
            if (account.Staff is null)
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
            if (account.Customer is null)
            {
                throw new NotFoundException("Customer profile not found");
            }

            userId = account.Customer.Id;
            role = account.Customer.Type.ToString();
            name = account.Customer.Name;
            phone = account.Customer.Phone;
        }

        string accessToken = jwtProvider.Generate(userId, account.Id, role, name, phone, account.Email);

        List<RefreshToken> existingTokens = await context.RefreshTokens
            .Where(x => x.AccountId == account.Id && !x.IsRevoked)
            .ToListAsync(cancellationToken);

        existingTokens.ForEach(x => x.IsRevoked = true);

        string refreshToken = refreshTokenGenerator.Generate();
        string refreshTokenHash = refreshTokenHasher.Hash(refreshToken);

        RefreshToken refreshTokenEntity = new()
        {
            Token = refreshTokenHash,
            AccountId = account.Id,
            ExpiredAt = DateTimeOffset.UtcNow.AddDays(7),
            IsRevoked = false
        };

        context.RefreshTokens.Add(refreshTokenEntity);
        await context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, refreshToken);
    }
}
