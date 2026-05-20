namespace Application.Features.Auth.Commands.Logout;

public record LogoutCommand : IRequest;

public class LogoutCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        string? currentUserId = currentUser.Id;
        string? accountId = currentUser.AccountId;
        if (currentUserId == null || accountId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in");
        }

        if (!int.TryParse(accountId, out int accountIdValue))
        {
            throw new UnauthorizedAccessException("Invalid account ID");
        }

        await context.RefreshTokens
            .Where(rt => rt.AccountId == accountIdValue && !rt.IsRevoked)
            .ExecuteUpdateAsync(
                s => s.SetProperty(x => x.IsRevoked, true),
                cancellationToken);
    }
}
