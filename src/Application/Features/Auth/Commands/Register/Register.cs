namespace Application.Features.Auth.Commands.Register;

public record RegisterResponse(string AccessToken, string RefreshToken);

public record RegisterCommand(
    string Email,
    string Password,
    string Name,
    string Phone,
    string CitizenId,
    CustomerType CustomerType)
    : IRequest<RegisterResponse>;

public class RegisterCommandHandler(
    IApplicationDbContext context,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenHasher refreshTokenHasher)
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        string hashedPassword = passwordHasher.Hash(request.Password);

        Account account = new()
        {
            Email = request.Email,
            Password = hashedPassword,
            Status = AccountStatus.Active,
            Role = AccountRole.Customer
        };

        context.Accounts.Add(account);

        Customer customer = new()
        {
            Name = request.Name,
            Phone = request.Phone,
            CitizenId = request.CitizenId,
            Type = request.CustomerType,
            Account = account
        };

        context.Customers.Add(customer);

        string rawRefreshToken = refreshTokenGenerator.Generate();
        string hashedRefreshToken = refreshTokenHasher.Hash(rawRefreshToken);

        RefreshToken refreshToken = new()
        {
            Token = hashedRefreshToken,
            Account = account,
            ExpiredAt = DateTimeOffset.UtcNow.AddDays(7),
            IsRevoked = false
        };

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        string accessToken = jwtProvider.Generate(customer.Id, account.Id, request.CustomerType.ToString(),
            request.Name, request.Phone, request.Email);

        return new RegisterResponse(accessToken, rawRefreshToken);
    }
}
